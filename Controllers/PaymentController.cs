using hakathon.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace hakathon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        private readonly DataContext _context;

        public PaymentController(DataContext context)
        {
            _context = context;
        }
        // POST: api/Payment/Webhook
        [HttpPost("Webhook")]
        public async Task<IActionResult> Webhook([FromBody] dynamic payload)
        {
            
            string transactionId = payload.transactionId;
            int userId = (int)payload.userId;
            string actionType = payload.actionType;
            string description = payload.description;

            // Tạo bản ghi lịch sử giao dịch
            var history = new tblTransactionHistory
            {
                UserID = userId,
                ActionType = actionType,
                Description = $"TransactionId: {transactionId}. {description}",
                ActionDate = DateTime.Now
            };

            _context.tblTransactionHistories.Add(history);
            await _context.SaveChangesAsync();

            // Trả về xác nhận
            return Ok(new { message = "Webhook processed and transaction history saved" });
        }

        [HttpGet("TransactionHistory/{userId}")]
        public IActionResult TransactionHistory(int userId)
        {
            var histories = _context.tblTransactionHistories
                .Where(h => h.UserID == userId)
                .OrderByDescending(h => h.ActionDate)
                .Select(h => new
                {
                    h.TransactionID,
                    h.ActionType,
                    h.ActionDate,
                    h.Description,
                    h.DocumentID
                })
                .ToList();

            if (histories.Count == 0)
                return NotFound(new { message = "Không có lịch sử giao dịch cho user này." });

            return Ok(histories);
        }

		[HttpGet("TransactionStatus/{transactionId}")]
		public async Task<IActionResult> GetTransactionStatus(string transactionId)
		{
			// Giả sử transactionId là string, còn trong model anh dùng int TransactionID thì sửa lại cho phù hợp nhé
			// Nếu transactionId là int thì đổi thành int và parse

			// Nếu transactionId là int
			if (!int.TryParse(transactionId, out int transId))
			{
				return BadRequest(new { message = "TransactionID không hợp lệ" });
			}

			var transaction = await _context.tblTransactionHistories
				.Where(t => t.TransactionID == transId)
				.OrderByDescending(t => t.ActionDate)
				.FirstOrDefaultAsync();

			if (transaction == null)
			{
				return NotFound(new { message = "Không tìm thấy giao dịch" });
			}

			// Ví dụ: giả định ActionType chứa trạng thái thanh toán, ví dụ "Success" hoặc "Pending"
			// Nếu anh có cột khác để lưu trạng thái chính xác thì dùng cột đó

			return Ok(new
			{
				transactionId = transaction.TransactionID,
				userId = transaction.UserID,
				documentId = transaction.DocumentID,
				status = transaction.ActionType, // ví dụ: "Success", "Pending"
				actionDate = transaction.ActionDate,
				description = transaction.Description
			});
		}
	}
}
