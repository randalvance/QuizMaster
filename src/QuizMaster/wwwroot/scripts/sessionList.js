var SessionListUtilities = {
    showScore: function(gradeAverage, scoreLabel) {
        var gradeOfTheDay = new ProgressBar.Circle('#daily-grade-container', {
            strokeWidth: 10,
            trailWidth: 1,
            text: {
                value: scoreLabel,
                className: 'daily-grade-label'
            },
            duration: 800,
            easing: 'easeInOut',
            from: { color: '#f00' },
            to: { color: '#0f0' },
            step: function (state, circle, attachment) {
                circle.path.setAttribute('stroke', state.color);
            }
        });

        gradeOfTheDay.animate(gradeAverage / 100);
    }
};