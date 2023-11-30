


function validateregister(nombre, correo, contraseña,telefono,direccion,sexo) {
    var nombr = document.getElementById(nombre).value;
    var email = document.getElementById(correo).value;
    var pass = document.getElementById(contraseña).value;
    var regexemail = /^[a-zA-Z0-9._%+-]+@gmail\.com$/;
    var regexpass = /^(?=.*[A-Z])(?=.*[!@#$%^&*(),.?\"':{}|<>])(.{8,})$/;
    var regexfono = /^(?!.*\+569)[98756]\d{8}$/;
    var fono = document.getElementById(telefono).value;
    var direccio = document.getElementById(direccion).value;
    var sexoselected = document.querySelector('input[name="' + sexo + '"]:checked');
    if (nombr == "") {
        alert("Debe ingresar un nombre");
        return false;
    }
    if (!regexemail.test(email) || email =="") {
        alert("Debe ingresar un correo con dominio Gmail");
        return false;
    }
    if (pass == "" || !regexpass.test(pass)) {
        alert("Debe ingresar una contraseña que tenga 8 caracterees, al menos una mayuscula y un caracter especial");
        return false;
    }
    if (fono == "" || !regexfono.test(fono)) {
        alert("Debe ingresar un número de telefono valido");
        return false;
    }
    if (direccio == "") {
        alert("Debe ingresar una dirección");
        return false;
    }
    if (!sexoselected) {
        alert("Debe seleccionar un género");
        return false;
    }
};


function validarregistrolibro(isbn,nombre,autor,precio,stock,anio,editorial,descripcion,imagen,fisico,digital) {

    var cod_libro = document.getElementById(isbn).value;
    var nombr = document.getElementById(nombre).value;
    var aut = document.getElementById(autor).value;
    var tlDigital = document.getElementById(digital);
    var tlFisico = document.getElementById(fisico);
    var pre = document.getElementById(precio).value;
    var st = document.getElementById(stock).value;
    var an = document.getElementById(anio).value;
    var edi = document.getElementById(editorial).options[document.getElementById(editorial).selectedIndex].value;
    var des = document.getElementById(descripcion).value;
    var img = document.getElementById(imagen);
    var categoriaCheckboxes = document.querySelectorAll('input[name="Categorias[]"]:checked');
    

    if (cod_libro == "") {
        alert("Debe ingresar un ISBN");
        return false;
    }


    if (nombr == "") {
        alert("Debe ingresar un nombre");
        return false;
    }

    if (aut == "") {
        alert("Debe ingresar un autor");
        return false;
    }

    if (!tlDigital.checked && !tlFisico.checked) {
        alert("Debe seleccionar un tipo de libro");
        return false;
    }

    if (pre == "") {
        alert("Debe ingresar un precio");
        return false;
    }

    if (st == "") {
        alert("Debe ingresar un stock");
        return false;
    }

    if (an == "") {
        alert("Debe ingresar un año");
        return false;
    }
    if (edi == "") {
        alert("Debe ingresar un editorial");
        return false;
    }
    if (categoriaCheckboxes.length === 0) {
        alert("Debe seleccionar al menos una categoría");
        return false;
    }

    if (des == "") {
        alert("Debe ingresar un descripción");
        return false;
    }

    if (!img.files || img.files.length === 0) {
        alert("Debe seleccionar una imagen");
        return false;
    }

    return true;

};