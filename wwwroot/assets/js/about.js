// SenseLib About Page Scripts

document.addEventListener('DOMContentLoaded', function() {
    // Animation for sections when they come into view
    const animateSections = () => {
        const sections = document.querySelectorAll('.senso-mission-section, .senso-team-section, .senso-tech-section, .senso-cta-section');
        
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('senso-animate-in');
                    observer.unobserve(entry.target);
                }
            });
        }, {
            threshold: 0.15
        });
        
        sections.forEach(section => {
            observer.observe(section);
        });
    };
    
    // Animate mission cards sequentially
    const animateMissionCards = () => {
        const cards = document.querySelectorAll('.senso-mission-card');
        
        const observer = new IntersectionObserver((entries) => {
            if (entries[0].isIntersecting) {
                cards.forEach((card, index) => {
                    setTimeout(() => {
                        card.classList.add('senso-card-animate');
                    }, index * 200);
                });
                observer.unobserve(entries[0].target);
            }
        }, {
            threshold: 0.5
        });
        
        if (cards.length > 0) {
            observer.observe(document.querySelector('.senso-mission-content'));
        }
    };
    
    // Animate team members with a slight delay between each
    const animateTeamMembers = () => {
        const members = document.querySelectorAll('.senso-team-member');
        
        const observer = new IntersectionObserver((entries) => {
            if (entries[0].isIntersecting) {
                members.forEach((member, index) => {
                    setTimeout(() => {
                        member.classList.add('senso-member-animate');
                    }, index * 150);
                });
                observer.unobserve(entries[0].target);
            }
        }, {
            threshold: 0.2
        });
        
        if (members.length > 0) {
            observer.observe(document.querySelector('.senso-team-content'));
        }
    };
    
    // Animate tech items with staggered appearance
    const animateTechItems = () => {
        const techGroups = document.querySelectorAll('.senso-tech-group');
        
        techGroups.forEach(group => {
            const observer = new IntersectionObserver((entries) => {
                if (entries[0].isIntersecting) {
                    const items = group.querySelectorAll('.senso-tech-item');
                    items.forEach((item, index) => {
                        setTimeout(() => {
                            item.classList.add('senso-tech-animate');
                        }, index * 100);
                    });
                    observer.unobserve(entries[0].target);
                }
            }, {
                threshold: 0.2
            });
            
            observer.observe(group);
        });
    };
    
    // Add smooth scrolling for anchor links
    const setupSmoothScrolling = () => {
        document.querySelectorAll('a[href^="#"]').forEach(anchor => {
            anchor.addEventListener('click', function(e) {
                e.preventDefault();
                
                const targetId = this.getAttribute('href');
                if (targetId === '#') return;
                
                const targetElement = document.querySelector(targetId);
                if (targetElement) {
                    window.scrollTo({
                        top: targetElement.offsetTop - 100,
                        behavior: 'smooth'
                    });
                }
            });
        });
    };
    
    // Dynamic counter for statistics if added in the future
    const setupCounters = () => {
        const counters = document.querySelectorAll('.senso-counter');
        
        if (counters.length === 0) return;
        
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const target = entry.target;
                    const countTo = parseInt(target.getAttribute('data-count'));
                    let count = 0;
                    const updateCounter = () => {
                        if (count < countTo) {
                            count += Math.ceil(countTo / 100);
                            if (count > countTo) count = countTo;
                            target.innerText = count.toLocaleString();
                            setTimeout(updateCounter, 20);
                        }
                    };
                    updateCounter();
                    observer.unobserve(target);
                }
            });
        }, {
            threshold: 0.5
        });
        
        counters.forEach(counter => {
            observer.observe(counter);
        });
    };
    
    // Add hover effect to team social links
    const setupTeamSocialHover = () => {
        const socialLinks = document.querySelectorAll('.senso-social-link');
        
        socialLinks.forEach(link => {
            link.addEventListener('mouseenter', function() {
                this.style.transform = 'scale(1.2)';
            });
            
            link.addEventListener('mouseleave', function() {
                this.style.transform = 'scale(1)';
            });
        });
    };
    
    // Initialize all animations and interactions
    const init = () => {
        // Add required CSS for animations
        const style = document.createElement('style');
        style.textContent = `
            .senso-animate-in {
                animation: fadeIn 0.8s ease-out forwards;
            }
            
            .senso-card-animate {
                animation: slideUp 0.6s ease-out forwards;
            }
            
            .senso-member-animate {
                animation: fadeScale 0.7s ease-out forwards;
            }
            
            .senso-tech-animate {
                animation: popIn 0.5s ease-out forwards;
            }
            
            @keyframes fadeIn {
                from { opacity: 0; transform: translateY(30px); }
                to { opacity: 1; transform: translateY(0); }
            }
            
            @keyframes slideUp {
                from { opacity: 0; transform: translateY(40px); }
                to { opacity: 1; transform: translateY(0); }
            }
            
            @keyframes fadeScale {
                from { opacity: 0; transform: scale(0.8); }
                to { opacity: 1; transform: scale(1); }
            }
            
            @keyframes popIn {
                0% { opacity: 0; transform: scale(0.5); }
                70% { transform: scale(1.05); }
                100% { opacity: 1; transform: scale(1); }
            }
            
            .senso-mission-card, .senso-team-member, .senso-tech-item {
                opacity: 0;
            }
        `;
        document.head.appendChild(style);
        
        // Call all setup functions
        animateSections();
        animateMissionCards();
        animateTeamMembers();
        animateTechItems();
        setupSmoothScrolling();
        setupCounters();
        setupTeamSocialHover();
    };
    
    // Run initialization
    init();
});