var QuizEditUtilities = {
    initialize: function () {
        var quizTypeDropDown = $('.quiz-type-dropdown');
        quizTypeDropDown.change(this.showOrHideChoices);
        this.showOrHideChoices();
    },

    showOrHideChoices: function () {
        var quizTypeCombo = $('.quiz-type-dropdown');
        var choiceContainers = $('.choices-input-container');
        var choiceInputs = choiceContainers.find('.choices-text-box');

        if (quizTypeCombo.val() != "MultipleChoice") {
            choiceInputs.val('');
            choiceContainers.hide();
        } else {
            choiceContainers.show();
        }
    }
};