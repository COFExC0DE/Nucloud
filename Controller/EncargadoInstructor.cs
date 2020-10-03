﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sala.Model;

namespace Sala.Controller {
    class EncargadoInstructor {
        public List<Instructor> Instructores { get; set; }

        public EncargadoInstructor() {
            Instructores = new List<Instructor>();
        }

        internal void agregarInstructor(int i, string n, int t, string e) {
            Instructores.Add(new Instructor() {
                ID = i,
                Nombre = n,
                Telefono = t,
                Email = e
            });
        }

        public Instructor obtenerInstructor(int cod) {
            return Instructores.Find(x => (x.ID == cod) && (x.activo == true));
        }

        public List<Instructor> obtenerTodosInstructor(){
            return Instructores;
        }

        public void desactivarInstructor(int pId){
            obtenerInstructor(pId).activo = false;
        }

        public void agregarServicio(int codInstructor, Servicio servicio) {
            obtenerInstructor(codInstructor).Servicios.Add(servicio);
        }

        public void eliminarServicio(int codInstructor, int codServicio) {
            obtenerInstructor(codInstructor).Servicios.RemoveAll(x => x.Codigo == codServicio);
        }
    }
}
