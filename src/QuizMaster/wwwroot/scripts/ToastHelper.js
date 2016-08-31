var ToastHelper = {
    showNotification: function (toastOptionsJson) {
        if (toastOptionsJson != '') {
            var toastOptions = JSON.parse(toastOptionsJson);
            toastr.options.positionClass = 'toast-bottom-right';
            toastr.success(toastOptions.Message, toastOptions.Title);
        }
    }
};
