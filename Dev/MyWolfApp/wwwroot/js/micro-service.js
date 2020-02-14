 $(document).ready(function () {

    $('#ServiceDataForm').submit(function (e) {

        var formData = new FormData(this);

        $.ajax({
            url: '/api/values")',
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: formData,
            contentType: false,
            processData: false,
            success: function (result) {
                console.log(result)
             // here in result you will get your data
            },
            error: function (result) {
            console.error(result);s
            }
        });
        e.preventDefault();
    });
});
