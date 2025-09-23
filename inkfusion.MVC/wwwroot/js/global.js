// Dil ve Tema için metinler
const texts = {
    'tr': {
        'home': 'Ana Sayfa', 'about': 'Hakkımızda', 'services': 'Hizmetler', 'gallery': 'Galeri', 'artists': 'Sanatçılar', 'testimonials': 'Yorumlar', 'contact': 'İletişim',
        'hero-title': 'Sanatını <span class="text-accent">Vücudunda</span> Taşı',
        'hero-subtitle': 'Profesyonel dövme sanatçıları ile hayalindeki dövmeyi gerçeğe dönüştürüyoruz.',
        'book-now': 'Randevu Al',
        'view-work': 'İşlerimize Bak',
        'about-title': 'Hakkımızda', 'about-text': '10 yılı aşkın tecrübemizle, İstanbul\'un en gözde dövme stüdyosu olarak hizmet vermekteyiz. Uzman ekibimiz ve steril çalışma ortamımızla müşterilerimize en kaliteli hizmeti sunmaktan gurur duyuyoruz.',
        'feature1-title': 'Özel Tasarımlar', 'feature1-text': 'Tamamen size özel ve benzersiz çizimler.',
        'feature2-title': 'Kaliteli İşçilik', 'feature2-text': 'En kaliteli mürekkep ve ekipmanlarla çalışıyoruz.',
        'feature3-title': 'Uzman Ekip', 'feature3-text': 'Alanında uzman deneyimli dövme sanatçıları.',
        'services-title': 'Hizmetlerimiz', 'services-subtitle': 'Size özel çözümler sunuyoruz',
        'service1-title': 'Özel Tasarım', 'service1-text': 'Hayalinizdeki dövmeyi birlikte tasarlayalım. Tamamen size özel ve özgün çizimler.',
        'service2-title': 'Renkli Dövmeler', 'service2-text': 'Canlı ve kalıcı renklerle hayalinizdeki dövmeyi sanat eserine dönüştürüyoruz.',
        'service3-title': 'Cover Up (Dövme Kapatma)', 'service3-text': 'İstemediğiniz dövmeleri, profesyonel dokunuşlarla yepyeni bir sanat eserine dönüştürüyoruz.',
        'gallery-title': 'Galeri', 'gallery-subtitle': 'Yaptığımız bazı çalışmalar',
        'view-all-gallery': 'Daha Fazlası İçin Tıklayın',
        'artists-title': 'Sanatçılarımız', 'artists-subtitle': 'Uzman ekibimizle tanışın',
        'artist1-name': 'Yiğit Kırkyıldız', 'artist1-specialty': 'Realistik Dövmeler Uzmanı',
        'artist2-name': 'Onur Sarı', 'artist2-specialty': 'Geleneksel Dövmeler Uzmanı',
        'artist3-name': 'Ramazan Savaş Çinioğlu', 'artist3-specialty': 'Minimal Dövmeler Uzmanı',
        'testimonials-title': 'Müşteri Yorumları', 'testimonials-subtitle': 'Müşterilerimizin deneyimleri',
        'testimonial1-text': 'Harika bir deneyimdi. Temizlik ve hijyen konusunda çok titizler. Dövmem tam istediğim gibi oldu.', 'testimonial1-author': 'Deniz K.', 'testimonial1-tattoo-style': 'Kol Dövmesi',
        'testimonial2-text': 'Sanatçılar gerçekten çok yetenekli. Özel tasarım sürecinde tüm isteklerimi dikkate aldılar.', 'testimonial2-author': 'Ahmet S.', 'testimonial2-tattoo-style': 'Özel Tasarım',
        'testimonial3-text': 'Dövme silme işlemi yaptırdım. Sonuç mükemmel, neredeyse hiç iz kalmadı. Teşekkürler!', 'testimonial3-author': 'Elif Y.', 'testimonial3-tattoo-style': 'Dövme Kapatma',
        'contact-title': 'İletişim', 'contact-subtitle': 'Randevu ve bilgi almak için bize ulaşın',
        'contact-name-placeholder': 'Adınız Soyadınız', 'contact-email-placeholder': 'E-posta Adresiniz', 'contact-subject-placeholder': 'Konu', 'contact-message-placeholder': 'Mesajınız', 'contact-submit-btn': 'Gönder',
        'footer-about-text': 'Sanatını seven, kaliteyi önemseyen bir dövme stüdyosu. Sizlere en iyi hizmeti sunmak için buradayız.',
        'contact-info-title': 'İletişim', 'address': 'Caferağa, Nail Bey Sk. No: 3, 34710 Kadıköy/İstanbul', 'phone': '+90 531 288 01 61', 'email': 'ciniogluserap@gmail.com',
        'social-media-title': 'Sosyal Medya', 'privacy': 'Gizlilik',
        'gallery-page-title': 'Sanat Galerimiz', 'gallery-page-subtitle': 'Her biri bir hikaye, her biri bir sanat eseri',
        'filter-all': 'Tümü', 'filter-realistic': 'Realistik Dövmeler', 'filter-dermal': 'Dermal & Piercingler',
        'filter-traditional': 'Geleneksel', 'filter-colored': 'Renkli', 'filter-geometric': 'Geometrik'
    },
    'en': {
        'home': 'Home', 'about': 'About Us', 'services': 'Services', 'gallery': 'Gallery', 'artists': 'Artists', 'testimonials': 'Testimonials', 'contact': 'Contact',
        'hero-title': 'Carry Your <span class="text-accent">Art</span> on Your Body',
        'hero-subtitle': 'We turn your dream tattoo into reality with professional tattoo artists.',
        'book-now': 'Book Now',
        'view-work': 'View Our Work',
        'about-title': 'About Us', 'about-text': 'With over 10 years of experience, we are proud to serve as one of Istanbul\'s most popular tattoo studios. With our expert team and sterile work environment, we offer our customers the highest quality service.',
        'feature1-title': 'Custom Designs', 'feature1-text': 'Completely unique and custom drawings for you.',
        'feature2-title': 'Quality Craftsmanship', 'feature2-text': 'We work with the highest quality inks and equipment.',
        'feature3-title': 'Expert Team', 'feature3-text': 'Experienced tattoo artists who are experts in their field.',
        'services-title': 'Our Services', 'services-subtitle': 'We offer tailored solutions for you',
        'service1-title': 'Custom Design', 'service1-text': 'Let\'s design your dream tattoo together. Fully customized and original drawings for you.',
        'service2-title': 'Colored Tattoos', 'service2-text': 'We turn your dream tattoo into a work of art with vibrant and permanent colors.',
        'service3-title': 'Cover Up (Tattoo Cover Up)', 'service3-text': 'We transform unwanted tattoos into a brand new work of art with professional touches.',
        'gallery-title': 'Gallery', 'gallery-subtitle': 'Some of our works',
        'view-all-gallery': 'Click for More',
        'artists-title': 'Our Artists', 'artists-subtitle': 'Meet our expert team',
        'artist1-name': 'Ayşe Yılmaz', 'artist1-specialty': 'Realistic Tattoo Specialist',
        'artist2-name': 'Mehmet Kaya', 'artist2-specialty': 'Traditional Tattoo Specialist',
        'artist3-name': 'Zeynep Demir', 'artist3-specialty': 'Minimal Tattoo Specialist',
        'testimonials-title': 'Client Testimonials', 'testimonials-subtitle': 'Our clients\' experiences',
        'testimonial1-text': 'It was a great experience. They are very meticulous about cleanliness and hygiene. My tattoo turned out exactly as I wanted.', 'testimonial1-author': 'Deniz K.', 'testimonial1-tattoo-style': 'Arm Tattoo',
        'testimonial2-text': 'The artists are truly talented. They took all my requests into account during the custom design process.', 'testimonial2-author': 'Ahmet S.', 'testimonial2-tattoo-style': 'Custom Design',
        'testimonial3-text': 'I had a tattoo removal procedure. The result is perfect, almost no trace left. Thanks!', 'testimonial3-author': 'Elif Y.', 'testimonial3-tattoo-style': 'Tattoo Removal',
        'contact-title': 'Contact', 'contact-subtitle': 'Contact us for an appointment or information',
        'contact-name-placeholder': 'Your Name Surname', 'contact-email-placeholder': 'Your Email Address', 'contact-subject-placeholder': 'Subject', 'contact-message-placeholder': 'Your Message', 'contact-submit-btn': 'Send',
        'footer-about-text': 'A tattoo studio that loves art and values quality. We are here to provide you with the best service.',
        'contact-info-title': 'Contact', 'address': 'Caferağa, Nail Bey Sk. No: 3, 34710 Kadıköy/İstanbul', 'phone': '+90 531 288 01 61', 'email': 'ciniogluserap@gmail.com',
        'social-media-title': 'Social Media', 'privacy': 'Privacy',
        'gallery-page-title': 'Our Art Gallery', 'gallery-page-subtitle': 'Every piece is a story, every piece is a work of art',
        'filter-all': 'All', 'filter-realistic': 'Realistic Tattoos', 'filter-dermal': 'Dermal & Piercings',
        'filter-traditional': 'Traditional', 'filter-colored': 'Colored', 'filter-geometric': 'Geometric'
    },
};

document.addEventListener('DOMContentLoaded', function () {
    // Tema (Theme) Toggle Logic
    const themeToggle = document.getElementById('themeToggle');
    const body = document.body;
    const savedTheme = localStorage.getItem('theme');

    if (savedTheme) {
        body.setAttribute('data-theme', savedTheme);
        updateThemeIcon(savedTheme);
    }

    themeToggle.addEventListener('click', function () {
        const currentTheme = body.getAttribute('data-theme');
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
        body.setAttribute('data-theme', newTheme);
        localStorage.setItem('theme', newTheme);
        updateThemeIcon(newTheme);
    });

    function updateThemeIcon(theme) {
        const themeIcon = themeToggle.querySelector('i');
        themeIcon.className = theme === 'dark' ? 'fas fa-sun' : 'fas fa-moon';
    }

    // Dil (Language) Toggle Logic
    const langToggle = document.getElementById('langToggle');
    let currentLang = localStorage.getItem('lang') || 'tr';
    updateLang(currentLang);

    langToggle.addEventListener('click', function () {
        currentLang = currentLang === 'tr' ? 'en' : 'tr';
        localStorage.setItem('lang', currentLang);
        updateLang(currentLang);
    });

    function updateLang(lang) {
        document.querySelectorAll('[data-lang-key]').forEach(element => {
            const key = element.getAttribute('data-lang-key');
            if (texts[lang] && texts[lang][key]) {
                element.innerHTML = texts[lang][key];
            }
        });

        document.querySelectorAll('[placeholder]').forEach(element => {
            const key = element.getAttribute('data-lang-key');
            if (texts[lang] && texts[lang][key]) {
                element.placeholder = texts[lang][key];
            }
        });

        // Update social icons hover text
        document.querySelectorAll('.fixed-social-icons a').forEach(icon => {
            const hoverText = icon.getAttribute(`data-hover-text-${lang}`);
            if (hoverText) {
                icon.setAttribute('data-hover-text', hoverText);
            }
        });

        langToggle.textContent = lang.toUpperCase();
        document.documentElement.lang = lang;
    }
});