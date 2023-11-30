function mostrarSiguienteFormulario() {
    var primerForm = document.getElementById('primer-form');
    var segundoForm = document.getElementById('segundo-form');
    var tercerForm = document.getElementById('tercer-form');
    var continuarBtn = document.getElementById('continuarBtn');
    var sexoRow = document.getElementById('sexoRow');

    if (primerForm.style.display !== 'none') {
        primerForm.classList.remove('fade-in');
        primerForm.classList.add('fade-out');
        setTimeout(function () {
            primerForm.style.display = 'none';
            segundoForm.style.display = 'block';
            segundoForm.classList.remove('fade-out');
            segundoForm.classList.add('fade-in');
        }, 500);
    } else if (segundoForm.style.display !== 'none') {
        segundoForm.classList.remove('fade-in');
        segundoForm.classList.add('fade-out');
        setTimeout(function () {
            segundoForm.style.display = 'none';
            tercerForm.style.display = 'block';
            tercerForm.classList.remove('fade-out');
            tercerForm.classList.add('fade-in');
            continuarBtn.innerText = 'Guardar';
            // Hacer visible los elementos de sexo
            sexoRow.style.display = 'flex';
        }, 500);
    } else {
        // Aquí puedes realizar acciones adicionales antes de enviar el formulario
        // Cambiar el tipo del botón a 'submit'
        continuarBtn.type = 'submit';
    }
}
