var TakeQuizUtilities = {
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
            to: { color: '#0f0' },
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
            to: { color: '#0f0' },
            step: function (state, circle, attachment) {
                circle.path.setAttribute('stroke', state.color);
            }
        });
        
        currentScoreProgressCircle.animate(retryAnswerCount * 0.1);
    }
};