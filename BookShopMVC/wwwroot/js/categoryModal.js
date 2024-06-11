document.addEventListener('DOMContentLoaded', function () {
    var categoryNameInput = document.getElementById('categoryName');

    categoryNameInput.addEventListener('input', function (e) {
        var value = e.target.value;
        e.target.value = value.replace(/[^A-Za-z]/g, '');
    });
});
