// ==========================================
// 1. BASE DE DATOS SIMULADA
// ==========================================
const pacientesRegistrados = [
    { dni: "71234567", nombre: "Juan Pérez Milla", edad: 28, seguro: "Asegurado - SIS" },
    { dni: "12345678", nombre: "María Garcia", edad: 35, seguro: "Particular" },
    { dni: "44556677", nombre: "Pedro Alcántara", edad: 42, seguro: "Asegurado - EsSalud" }
];

const medicosPorEspecialidad = {
    cardiologia: [{ id: "med-01", nombre: "Dr. Roberto Pérez" }],
    pediatria: [{ id: "med-02", nombre: "Dra. Ana Gómez" }],
    medicina_general: [
        { id: "med-03", nombre: "Dr. Carlos Ruiz" },
        { id: "med-04", nombre: "Dra. Sofía Castro" }
    ]
};

const horariosBase = ["08:00", "09:00", "10:00", "11:00", "15:00", "16:00"];
const citasOcupadas = [
    { idMedico: "med-01", fecha: "2026-05-15", hora: "10:00" } 
];

// ==========================================
// 2. REFERENCIAS Y SELECTORES
// ==========================================
const contenedorMensajes = document.getElementById('contenedor-mensajes');
const formCita = document.getElementById('form-registrar-cita');
const inputBusqueda = document.getElementById('buscar-paciente');
const btnBuscar = document.getElementById('btn-buscar-paciente');
const spanEstado = document.getElementById('estado-paciente');
const visorInfoPaciente = document.getElementById('info-paciente-detallada');

const selectEspecialidad = document.getElementById('especialidad');
const selectMedico = document.getElementById('medico');
const inputFecha = document.getElementById('fecha');
const selectHora = document.getElementById('hora');

let pacienteSeleccionadoValido = null;

// ==========================================
// 3. FUNCIONES
// ==========================================

function mostrarMensaje(texto, tipo) {
    contenedorMensajes.innerHTML = `<div class="alerta alerta-${tipo}">${texto}</div>`;
    window.scrollTo(0, 0); // Sube para que el usuario vea el mensaje
    setTimeout(() => { contenedorMensajes.innerHTML = ''; }, 5000);
}

// CA-03: Validar y Mostrar información del paciente
// Localiza esta sección en tu archivo app.js
btnBuscar.addEventListener('click', () => {
    const termino = inputBusqueda.value.trim(); // Eliminamos espacios en blanco
    
    // 1. NUEVA VALIDACIÓN: Si el campo está vacío, lanzar alerta y detener ejecución
    if (termino === "") {
        spanEstado.textContent = " ⚠️ Ingrese un DNI";
        spanEstado.style.color = "orange";
        visorInfoPaciente.style.display = "none";
        mostrarMensaje('Debe ingresar un Documento de Identidad para realizar la búsqueda.', 'advertencia');
        return; // Detiene la función aquí
    }

    // 2. Lógica de búsqueda normal
    const encontrado = pacientesRegistrados.find(p => 
        p.dni === termino || p.nombre.toLowerCase().includes(termino.toLowerCase())
    );

    if (encontrado) {
        pacienteSeleccionadoValido = encontrado;
        spanEstado.textContent = " ✔️ Paciente Verificado";
        spanEstado.style.color = "green";
        
        visorInfoPaciente.style.display = "block";
        visorInfoPaciente.innerHTML = `
            <strong>Nombre:</strong> ${encontrado.nombre}<br>
            <strong>Edad:</strong> ${encontrado.edad} años<br>
            <strong>Seguro:</strong> ${encontrado.seguro}
        `;
    } else {
        // CA-03: Validar paciente no registrado
        pacienteSeleccionadoValido = null;
        spanEstado.textContent = " ❌ No registrado";
        spanEstado.style.color = "red";
        visorInfoPaciente.style.display = "none";
        mostrarMensaje('No se puede registrar. El paciente no existe en el sistema.', 'error');
    }
});

// Lógica para habilitar médicos y horarios (RN-02, RN-03, CA-01)
selectEspecialidad.addEventListener('change', () => {
    const medicos = medicosPorEspecialidad[selectEspecialidad.value] || [];
    selectMedico.innerHTML = '<option value="" disabled selected>Seleccione un médico</option>';
    medicos.forEach(m => {
        const opt = document.createElement('option');
        opt.value = m.id; opt.textContent = m.nombre;
        selectMedico.appendChild(opt);
    });
    selectMedico.disabled = false;
});

function actualizarDisponibilidad() {
    if (selectMedico.value && inputFecha.value) {
        selectHora.innerHTML = '<option value="" disabled selected>Seleccione hora</option>';
        let disponibles = 0;

        horariosBase.forEach(h => {
            const ocupada = citasOcupadas.some(c => 
                c.idMedico === selectMedico.value && c.fecha === inputFecha.value && c.hora === h
            );
            if (!ocupada) {
                const opt = document.createElement('option');
                opt.value = h; opt.textContent = h;
                selectHora.appendChild(opt);
                disponibles++;
            }
        });

        selectHora.disabled = false;
        if (disponibles === 0) {
            selectHora.innerHTML = '<option value="" disabled selected>Sin cupos</option>';
            selectHora.disabled = true;
            mostrarMensaje('CA-04: El horario no se encuentra disponible.', 'advertencia');
        }
    }
}

selectMedico.addEventListener('change', actualizarDisponibilidad);
inputFecha.addEventListener('change', actualizarDisponibilidad);

// Envío final (CA-02, CA-05, CA-06)
formCita.addEventListener('submit', (e) => {
    e.preventDefault();

    // Validar si buscó y encontró al paciente antes de enviar
    if (!pacienteSeleccionadoValido) {
        mostrarMensaje('Error: No se puede registrar. El paciente no existe en el sistema.', 'error');
        return;
    }

    // CA-02: Registro exitoso
    citasOcupadas.push({
        idMedico: selectMedico.value,
        fecha: inputFecha.value,
        hora: selectHora.value
    });

    mostrarMensaje('✅ La cita fue registrada correctamente.', 'exito');
    formCita.reset();
    visorInfoPaciente.style.display = "none";
    spanEstado.textContent = "";
});