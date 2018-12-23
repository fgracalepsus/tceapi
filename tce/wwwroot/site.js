const uri = "api/book";
let todos = null;
function getCount(data) {
    const el = $("#counter");
    let name = "livro";
    if (data) {
        if (data > 1) {
            name = "livros";
        }
        el.text(data + " " + name);
    } else {
        el.text("Nenhum " + name);
    }
}

$(document).ready(function () {
    getData();
});

function getData() {
    $.ajax({
        type: "GET",
        url: uri,
        cache: false,
        success: function (data) {
            const tBody = $("#livros");

            $(tBody).empty();

            getCount(data.length);

            $.each(data, function (key, item) {
                let _published = item.published;
                if (_published) {
                    _published = (new Date(_published)).toLocaleDateString("pt-BR");
                }
                const tr = $("<tr></tr>")
                    .append($("<td></td>").text(item.isbn))
                    .append($("<td></td>").text(item.name))
                    .append($("<td class='text-right'></td>").text(item.price))
                    .append($("<td></td>").text(_published))
                    .append($("<td></td>").text(item.avatar)) //.append($("<img/>").src(item.avatar)))
                    .append(
                        $("<td></td>").append(
                        $("<i class='fas fa-edit btn' title='Clique aqui para editar este registro.'></i>").on("click", function () {
                                openItem(item);
                            })
                        )
                    )
                    .append(
                        $("<td></td>").append(
                            $("<i class='fas fa-trash-alt btn' title='Clique aqui para remover este registro.'></i>").on("click", function () {
                                deleteItem(item.id);
                            })
                        )
                    );

                tr.appendTo(tBody);
            });

            todos = data;
        }
    });
}

function clearItem(isnew) {
    if (isnew) {
        $("#id").val("");
    }
    $("#isbn").val("");
    $("#name").val("");
    $("#price").val("");
    $("#published").val("");
    $("#avatar").val("");
}

function openItem(item) {
    $("#id").val(item.id);
    $("#isbn").val(item.isbn);
    $("#name").val(item.name);
    $("#price").val(item.price);
    $("#published").val(item.published);
    $("#avatar").val(item.avatar);
}

function nullify(val) {
    return val == "" ? null : val;
}

function saveItem() {
    console.log("saving...")
    let item = {
        isbn: nullify($("#isbn").val()),
        name: nullify($("#name").val()),
        price: nullify($("#price").val()),
        published: nullify($("#published").val()),
        avatar: nullify($("#avatar").val())
    };

    let _id = nullify($("#id").val());
    if (_id) {
        item.id = _id;
    }
    
    console.log(item);

    $.ajax({
        type: (_id ? "PUT" : "POST"),
        url: uri + (_id ? "/" + _id : ""),
        accepts: "application/json",
        contentType: "application/json",
        data: JSON.stringify(item),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Something went wrong!");
            console.log(jqXHR);
            console.log(textStatus);
            console.log(errorThrown);
        },
        success: function (result) {
            console.log("Item salvo");
            console.log(result);
            getData();
            clearItem(true);
        }
    });
}

function deleteItem(id) {
    if (confirm("Confirma a remoção deste item?")) {
        $.ajax({
            url: uri + "/" + id,
            type: "DELETE",
            success: function (result) {
                getData();
            }
        });
    }
}