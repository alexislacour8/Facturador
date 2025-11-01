window.Alertas = {
    mostrarExito: function (mensaje) {
        Swal.fire({
            icon: 'success',
            title: 'Éxito',
            text: mensaje,
            confirmButtonText: 'Aceptar'
        });
    },
    mostrarError: function (mensaje) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: mensaje,
            confirmButtonText: 'Cerrar'
        });
    },
    confirmar: async function (mensaje) {
        const result = await Swal.fire({
            title: 'Confirmar',
            text: mensaje,
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Sí',
            cancelButtonText: 'No'
        });
        return result.isConfirmed;
    }
};
