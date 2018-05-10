(function ($) {
            var buttons,
                estado = 0;

            function init() {                
                buttons = $('.glyphicon-plus, .glyphicon-minus');
                buttons.mas = buttons.eq(0);
                buttons.menos = buttons.eq(1);

                buttons.on('click', buttonClick);
                updateUI();
            }

            function buttonClick(e) {
                estado += (this === buttons.mas[0]) ? 1 : -1;
                updateUI();
                e.preventDefault();
            }

            function updateUI() {
                buttons.mas.toggle(estado == 0);
                buttons.menos.toggle(estado !== 0);
            }

            $(init);
        }(jQuery));