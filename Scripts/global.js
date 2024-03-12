!function () {
    var theme = localStorage.getItem('theme');
    if (theme) {
        $('body').attr('theme', theme);
        if (theme === 'dark') {
            $('#theme').removeClass('uil-sunset').addClass('uil-moonset');
        }
    }

    var accent = localStorage.getItem('accent');
    if (accent) {
        $('body').attr('accent', accent);
        if (accent === 'pink') {
            $('body').attr('accent', 'pink');
        }
        else if (accent === 'blue') {
            $('body').attr('accent', 'blue');
        }
        else if (accent === 'green') {
            $('body').attr('accent', 'green');
        }
    }
}();
$(document).on("click", '#theme', function () {
    if ($('#theme').hasClass('uil-sunset')) {
        $('#theme').removeClass('uil-sunset').addClass('uil-moonset');
        $('body').attr('theme', 'dark');
        localStorage.setItem('theme', 'dark');
    }
    else {
        $('#theme').addClass('uil-sunset').removeClass('uil-moonset');
        $('body').attr('theme', 'light');
        localStorage.setItem('theme', 'light');
    }
});

$(document).ready(function () {
    var bubblesEnabled = localStorage.getItem('bg-animation');
    if (bubblesEnabled === 'true') {
        const numberOfBubbles = parseInt(parseInt(window.innerWidth) / 140);
        const container = $('body');

        for (let i = 0; i < numberOfBubbles; i++) {
            const bubble = $('<div>').addClass('bubble');
            bubble.css('left', `${(i * 150) - 50}px`);
            if (i === 0) {
                bubble.css('left', '-50px');
            }
            let duration;
            do {
                duration = Math.random() * 100;
            } while (duration < 10 || duration > 20)
            bubble.css('animation-duration', `${duration}s`); // Randomize animation duration
            container.prepend(bubble);
        }
    }
});

$(document).on("click", '#logout', function () {
    $.ajax({
        "url": '/Home/LogOut',
        "type": "POST",
        "success": function () {
            window.location.href = '/Home/Login';
        }
    })
});