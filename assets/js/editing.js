var element;
var parent;

$(document).ready(function () {

    addEditingButton("");

    $(document).on("click", ".editButton", function () {

        element = $(this).next()[0];
        parent = $(this).closest("section")[0];
        var namee = $(element).attr("name");
        $(".modalTitle").html(namee);
        console.log(element);
        console.log(parent);
        var elementtype = $(element).prop("nodeName");

        if (elementtype == "IMG") {
            $("#editImage").modal("show");

            $("#Imageitle").val($(element).attr("title"));
            $("#ImageAlt").val($(element).attr("alt"));
        }
        else {
            $("#basicModalText").modal("show");

            $("#innerData").val($(element).text().trim().replace(/\s*[\r\n]+\s*/g, '\n')
                .replace(/(<[^\/][^>]*>)\s*/g, '$1')
                .replace(/\s*(<\/[^>]+>)/g, '$1')).select();



        }


    });


});

function addEditingButton(a) {
    if (a == "") {
        a = ` .editable`;
    }
    else {
        a = `#${a} .editable`;

    }


    $(a).before(
        `<button type = "button" class="btn btn-info editButton" style="z-index:999">edit</button>`

    )
}

function textEdit() {

    id = $(element).attr("id");
    content = $("#innerData").val();


    $.post("/admin/textEdit", { id: id, content: content })

        .done(function (res) {
            if (res.status) {

                id2 = $(parent).attr("id").split("-");

                $(parent).load(`/component/${id2[0]}/${id2[1]}`, function () {
                    addEditingButton(`${$(parent).attr("id")}`);
                });

            }

        });

}

function ImageEdit() {
    var form = $("#editImage").find("form");
    var formData = new FormData(form[0]);

    formData.append("id", $(element).attr("id"));

    $.ajax({
        url: "/admin/EditImage",
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,

    })

        .done(function (res) {
            if (res.status) {

                id3 = $(parent).attr("id").split("-");

                $(parent).load(`/component/${id3[0]}/${id3[1]}`, function () {

                    addEditingButton(`${$(parent).attr("id")}`);
                });

                $("#editImage").modal("hide");

                swal.fire({
                    title: "عملیات موفقیت آمیز بود",
                    text: res.message,
                    icon: "success"
                });

            }
            else {
                $("#editImage").modal("hide");

                swal.fire({
                    title: "عملیات ناموفق",
                    text: res.message,
                    icon: "error"
                });
            }

        });
}
