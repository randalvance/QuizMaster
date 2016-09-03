var TakeQuizUtilities = {

    initializeControls: function () {
        var form = $('#main-form');

        $('select.answer-options').change(function () {
            var answerOptions = $(this);
            var answerOptionsHidden = answerOptions.siblings('.answer-options-hidden');

            answerOptionsHidden.val(answerOptions.val());
            alert(answerOptionsHidden.val());
        });

        $('.submit-answers-button').click(function (e) {
            e.preventDefault()
            if (confirm('Are you sure you want to submit?')) {
                form.submit();
            }
        });
    },

    showScores: function(showFirstScore, showCurrentScore, correctAnswerCount, retryAnswerCount) {
        if (!showFirstScore) return;

        var firstScoreProgressCircle = new ProgressBar.Circle('#first-score-container', {
            strokeWidth: 10,
            trailWidth: 1,
            text: {
                value: correctAnswerCount,
                className: 'score-label'
            },
            duration: 800,
            easing: 'easeInOut',
            from: { color: '#f00' },
            to: { color: '#32C700' },
            step: function (state, circle, attachment) {
                circle.path.setAttribute('stroke', state.color);
            }
        });

        firstScoreProgressCircle.animate(correctAnswerCount * 0.1);

        if (!showCurrentScore) return;

        var currentScoreProgressCircle = new ProgressBar.Circle('#current-score-container', {
            strokeWidth: 10,
            trailWidth: 1,
            text: {
                value: retryAnswerCount,
                className: 'score-label'
            },
            duration: 800,
            easing: 'easeInOut',
            from: { color: '#f00' },
            to: { color: '#32C700' },
            step: function (state, circle, attachment) {
                circle.path.setAttribute('stroke', state.color);
            }
        });
        
        currentScoreProgressCircle.animate(retryAnswerCount * 0.1);
    }
};