using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.FreeGlut;
using Tao.OpenGl;
using System.Diagnostics;

namespace ProyectoGraficacion
{
    class Program
    {
        public static int x0 = 0, y0 = 0, alfa = 0, beta = 0;
        static float xcamara = 2;
        static float ycamara = 10;
        static float zcamara = 25;

        //Este stopWatch es para tratar de correr el juego a la misma velocidad en diferentes computadoras
        static Stopwatch stopWatch = new Stopwatch();

        static float VELOCIDAD_MOVMIENTO=.007f;
        static float VELOCIDADROTACIONLLANTAS = .9f;
        static float[] mat_ambient_verde = { 0.0f, 1.0f, 0.0f, 0.0f };
        static float[] mat_ambient_rojo = { 1.0f, 0.0f, 0.0f, 0.0f };
        static float[] mat_ambient_azul = { 0.0f, 0.0f, 1.0f, 0.0f };
        static float[] mat_ambient_amarillo = { 3.0f, 3.0f, 0.0f, 0.0f };
        static float[] mat_ambient_rojiso = { 10.0f, 1.0f, 0.0f, 0.0f };
        static float[] mat_ambient_negro = { 0, 0, 0, 0 };
        static float[] mat_ambient_gris = { 0.7f, 0.7f, 0.7f, 1.0f };
        static float[] mat_ambient_plateado = { 0.999f, 0.999f, 0.9999f, 0f };

        static Vector3 posicionCarro = new Vector3();
        static float rotacionCarroY = 0;//El carro solo puede rotar en Y 
        static float rotacionLlanta = 0;
        static float giroLLantasDelanteras = 0;
        static bool derechoDown=false;
        static bool atrasDown = false;
        static bool izqDown = false;
        static bool derDown = false;
        static void Main(string[] args)
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_RGB | Glut.GLUT_DEPTH);
            Glut.glutInitWindowSize(1280, 960);
            Glut.glutInitWindowPosition(300, 25);
            Glut.glutCreateWindow("Car in 3D by Yayo");
            Glut.glutKeyboardFunc(keyboardDown);
            Glut.glutKeyboardUpFunc(keyboardUp);
            Glut.glutMotionFunc(onMotion);
            Glut.glutMouseFunc(onMouse);
            Glut.glutIdleFunc(update);

            Glut.glutDisplayFunc(dibujarTocho);
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);
            Gl.glShadeModel(Gl.GL_SMOOTH);
            Gl.glEnable(Gl.GL_DEPTH_TEST); //para eliminar las caras ocultas
            Gl.glEnable(Gl.GL_NORMALIZE); //normaliza el vector para ombrear apropiadamente
            Gl.glClearColor(1.0f, 1.0f, 1.0f, 0.0f); //El color de fondo es gris
            Gl.glViewport(0, 0, 1280, 960);
            stopWatch.Start();
            Glut.glutMainLoop();


        }
        //Aqui se actualizan las posiciones, se checan colisiones, etc.
        public static void update()
        {
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            //Delta es el tiempo transcurrido desde la ultima iteracion. Todos nuestros movimientos dependeran del tiempo.
            float delta = (float)ts.TotalMilliseconds;
          //  Console.WriteLine("RunTime " + delta);


            float V0x = VELOCIDAD_MOVMIENTO * (float)Math.Cos(GradosARadianes(rotacionCarroY))*  delta;
            float V0z = VELOCIDAD_MOVMIENTO * (float)Math.Sin(GradosARadianes(rotacionCarroY)) * delta;
            Console.WriteLine("Vx= " + V0x + "  Vz= " + V0z);

            if (derechoDown)
            {
                rotacionLlanta += VELOCIDADROTACIONLLANTAS * delta;
                posicionCarro.x -= V0x;// VELOCIDAD_MOVMIENTO* delta;
                posicionCarro.z += V0z; 

                if (izqDown)
                    rotacionCarroY += 2;
                else if (derDown)
                    rotacionCarroY -= 2; 
            }
            if (atrasDown)
            {
                rotacionLlanta -= VELOCIDADROTACIONLLANTAS * delta;
                posicionCarro.x += V0x;// VELOCIDAD_MOVMIENTO* delta;
                posicionCarro.z -= V0z;// VELOCIDAD_MOVMIENTO * delta * RadianesAGrados(((float)Math.Cos(GradosARadianes(rotacionCarroY))));
                
                if (izqDown)
                    rotacionCarroY -= 2;
                else if (derDown)
                    rotacionCarroY += 2; 
            }
            if (izqDown)
            {
                giroLLantasDelanteras = 30;
                  
            }
            else if (derDown)
            {
                giroLLantasDelanteras = -30;
            }
            else
            {
                giroLLantasDelanteras = 0;
            }

            stopWatch.Reset();
            stopWatch.Start();
            Glut.glutPostRedisplay();

        }

        static void dibujarTocho()
        {
          


            float[] mat_diffuse = {0.6f, 0.6f, 0.6f, 0.0f};
            float[] mat_specular = { 1.0f, 1.0f, 1.0f, 1.0f };
            float[] mat_shininess = { 50.0f };

            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient_gris);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, mat_diffuse);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_SPECULAR, mat_specular);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_SHININESS, mat_shininess);
            // asigna la apropiada fuente de luz
            float[] lightIntensity = { 0.7f, 0.7f, 0.7f, 1.0f };
            //float[] lightIntensity = { 1.0f, 1.0f, 1.0f, 1.0f };
            //float[] light_position = { 2.0f, 2.0f, 3.0f, 0.0f };
            float[] light_position = {5.0f,10.0f, 0.0f, 0.0f};

            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, light_position);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, lightIntensity);
            //Asigna los apropiados materiales a las superficies
            //asigna la cámara
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();

            //Gl.glOrtho(-5, 5, -5, 5, 0.1, 100.0);
            Glu.gluPerspective(67, 48 / 32, .1f, 100);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            //Gl.gluLookAt(2.3, 2.3, 2, 0, 0.25, 0, 0.0, 1.0, 0.0);
            Glu.gluLookAt(xcamara, ycamara, zcamara, 0, 0, 0, 0.0, 1, 0);
            //comienza el dibujo
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT); // Limpia la pantalla

            Gl.glRotated(alfa, 1, 0, 0);
            Gl.glRotated(beta, 0, 1, 0);
            dibujarVertices();
            
            Gl.glPushMatrix();
            Gl.glTranslated(posicionCarro.x, posicionCarro.y, posicionCarro.z);
            Gl.glRotated(rotacionCarroY, 0, 1, 0);
            dibujarCarro();
            dibujarCarroceria();
            dibujarDefensaDelantera();
            dibjarCofre2();
           // dibujarCofre();
            Gl.glPopMatrix();
            Glut.glutSwapBuffers();

        }

        static void dibjarCofre2() {
            Gl.glPushMatrix();
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient_rojiso);
            Gl.glTranslated(0, 1.1, 0);
            Glut.glutSolidCube(2);
            Gl.glPopMatrix();
        }

        static void dibujarCofre()
        {
            Gl.glPushMatrix();


            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient_azul);

            Gl.glTranslated(-2,1.85f, 0);
            Gl.glRotatef(30, 0, 0, 1);
            Gl.glRotatef(230, 0, 1, 0);
            Gl.glRotatef(90, 1, 0, 0);
            Gl.glBegin(Gl.GL_TRIANGLE_FAN);
            float radioEscala = 3.4f;
            for (float i = 0; i <= 100; i+=1f)
            {
                double angle = GradosARadianes(i);
                double x = Math.Cos(angle);
                double y = Math.Sin(angle);
                Gl.glNormal3d(x * radioEscala, y * radioEscala, .5);
                Gl.glVertex3d(x * radioEscala, y * radioEscala, .5);

                Gl.glNormal3d(x * radioEscala, y * radioEscala, .5);
                Gl.glVertex3d(x * radioEscala, y * radioEscala, -.5f);

                Gl.glNormal3d(0,0,0);
                Gl.glVertex3d(0, 0,0);
            }
            Gl.glEnd();
            Gl.glPopMatrix();
        }

        static void dibujarLlanta(){

            //Dibujo llanta
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient_negro);
            Gl.glPushMatrix();
            Gl.glScaled(1, 1,2f);
            Glut.glutSolidTorus(.3f,.8f, 100, 100);
            Gl.glPopMatrix();

            //DibujarAroRin
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient_plateado);
            Gl.glPushMatrix();
            Gl.glTranslated(0, 0, .45f);
            Glut.glutSolidTorus(.1f, .70f, 100, 100);
            Gl.glPopMatrix();

            //Dibujar centro del rin
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient_plateado);
            Gl.glPushMatrix();
            Gl.glTranslated(0, 0, .2f);
            Glut.glutSolidCylinder(.15f, .3, 100, 100);
            Gl.glPopMatrix();

            //Dibujar adorno del rin
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient_negro);
            Gl.glPushMatrix();
            Gl.glTranslated(0, 0, .5f);
            Glut.glutSolidCylinder(.08f, .05f, 100, 100);
            Gl.glPopMatrix();


            //Parte donde se conecta el rin con el carro
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient_negro);
            Gl.glPushMatrix();
            Gl.glTranslated(0, 0, .3f);
            Glut.glutSolidCylinder(.45, .05, 100, 100);
            Gl.glPopMatrix();

            //Dibujar Rayos del Rin
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient_plateado);
            for (int i = 0; i < 10; i++) {
                Gl.glPushMatrix();
                Gl.glTranslated(0, 0, .47f);
                Gl.glRotatef(18 * i, 0, 0, 1);
                Gl.glScaled(1, 30, 1);
                Glut.glutSolidCube(.05f);
                Gl.glPopMatrix();
            }
           
        }

        static void dibujarDefensaDelantera() {
            Gl.glPushMatrix();


            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient_azul);

            Gl.glTranslated(-2, .3f, 0);
            Gl.glRotatef(230, 0, 1, 0);
            Gl.glRotatef(90, 1, 0, 0);
            Gl.glBegin(Gl.GL_QUAD_STRIP);
            float radioEscala = 3.4f;
            for (int i = 0; i <= 100; i++)
            {
                double angle = GradosARadianes(i);
                double x = Math.Cos(angle);
                double y = Math.Sin(angle);
                Gl.glNormal3d(x * radioEscala, y * radioEscala, -.5);
                Gl.glVertex3d(x * radioEscala, y * radioEscala, -.5);

                Gl.glNormal3d(x * radioEscala, y * radioEscala, .5);
                Gl.glVertex3d(x * radioEscala, y * radioEscala, .5);
            }
            Gl.glEnd();
            Gl.glPopMatrix();
        }

        static void dibujarGuaraFangos() {
            Gl.glPushMatrix();


            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient_azul);

            Gl.glTranslated(-2, 0, 0);
            Gl.glRotatef(-10, 0, 0, 1);
            Gl.glBegin(Gl.GL_QUAD_STRIP);
            float radioEscala = 1.26f;
            for (int i = 0; i <= 200; i += 1)
            {
                double angle = GradosARadianes(i);
                double x = Math.Cos(angle);
                double y = Math.Sin(angle);
                Gl.glNormal3d(x * radioEscala, y * radioEscala, -.5);
                Gl.glVertex3d(x * radioEscala, y * radioEscala, -.5);

                Gl.glNormal3d(x * radioEscala, y * radioEscala, .5);
                Gl.glVertex3d(x * radioEscala, y * radioEscala, .5);
            }
            Gl.glEnd();
            Gl.glPopMatrix();
        }

        static void dibujarCarro(){
            //Llanta delantera Izquierda
            Gl.glPushMatrix();
            Gl.glTranslated(-3, 0, 3);
            Gl.glRotatef(giroLLantasDelanteras, 0, 1, 0);
            Gl.glRotatef(rotacionLlanta, 0, 0, 1);
            dibujarLlanta();
            Gl.glPopMatrix();
            //Dibujo su GuardaFangos
            Gl.glPushMatrix();
            Gl.glTranslated(-1f, 0, 3);
            dibujarGuaraFangos();
            Gl.glPopMatrix();

            //Llanta delantera Derecha
            Gl.glPushMatrix();
            Gl.glTranslated(-3, 0, -3);
            Gl.glRotatef(giroLLantasDelanteras, 0, 1, 0);
            Gl.glRotatef(rotacionLlanta, 0, 0, 1);
            Gl.glRotatef(180, 0, 1, 0);
            dibujarLlanta();
            Gl.glPopMatrix();
            //Dibujo su GuardaFangos
            Gl.glPushMatrix();
            Gl.glTranslated(-1f, 0, -3);
            dibujarGuaraFangos();
            Gl.glPopMatrix();

            //Llanta trasera Izquierda
            Gl.glPushMatrix();
            Gl.glTranslated(3, 0, 3);
            Gl.glRotatef(rotacionLlanta, 0, 0, 1);
            dibujarLlanta();
            Gl.glPopMatrix();
            //Dibujo su GuardaFangos
            Gl.glPushMatrix();
            Gl.glTranslated(5f, 0, 3);
            dibujarGuaraFangos();
            Gl.glPopMatrix();

            //Llanta trasera Derecha
            Gl.glPushMatrix();
            Gl.glTranslated(3, 0, -3);
            Gl.glRotatef(rotacionLlanta, 0, 0, 1);
            Gl.glRotatef(180, 0, 1, 0);
            dibujarLlanta();
            Gl.glPopMatrix();
            //Dibujo su GuardaFangos
            Gl.glPushMatrix();
            Gl.glTranslated(5f, 0,-3);
            dibujarGuaraFangos();
            Gl.glPopMatrix();
          
        
        }

        static void dibujarVertices()
        {
            //Dibuja la pared con espesor y tapa = palno xz, esquina en el origen
            Gl.glPushMatrix();
            Gl.glLineWidth(2.0f);
            Gl.glDisable(Gl.GL_LIGHTING);
            Gl.glDisable(Gl.GL_LIGHT0);

            Gl.glBegin(Gl.GL_LINES);
            //x rojo
            Gl.glColor3d(1f, 0, 0);
            Gl.glVertex3d(-10, 0, 0);
            Gl.glVertex3d(10, 0, 0);
            //y verde
            Gl.glColor3d(0, 1, 0);
            Gl.glVertex3d(0, -10, 0);
            Gl.glVertex3d(0, 10, 0);
            //z azul
            Gl.glColor3d(0, 0, 1);
            Gl.glVertex3d(0, 0, -10);
            Gl.glVertex3d(0, 0, 10);

            Gl.glEnd();

            //glutWireCube(1.0);
            Gl.glPopMatrix();
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);
        }

        static void dibujarCarroceria() {
            Gl.glPushMatrix();
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient_azul);
            Gl.glTranslated(0, -.1f, 0);
            Gl.glScaled(35, 1, 70);
            Glut.glutSolidCube(.1f);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient_azul);
            Gl.glTranslated(0, -.1f, 0);
            Gl.glScaled(95, 1, 41);
            Glut.glutSolidCube(.1f);
            Gl.glPopMatrix();
        }

        static void keyboardDown(byte key, int x, int y)
        {
            char k = (char)key;
            switch (k)
            {
                case 'x':
                    xcamara -= 2;

                    break;
                case 'X':
                   xcamara += 2;
                    break;
                case 'z':
                    zcamara -= 2;
                    break;
                case 'Z':
                    zcamara += 2;
                    break;
                case 'y':
                    ycamara -= 2;
                    break;
                case 'Y':
                    ycamara += 2;
                    break;
                case 'w':
                case 'W':
                    derechoDown = true;
                  //  rotacionLlanta -= VELOCIDAD;
                    break;
                case 'S':
                case 's':
                    atrasDown = true;
                    //rotacionLlanta += VELOCIDAD;
                    break;
                case 'a':
                case 'A':
                    izqDown = true;
                   
                    break;
                case 'd':
                case 'D':
                    derDown = true;
                    break;
            }

            Console.WriteLine("X: " + xcamara);
            Console.WriteLine("Z: " + zcamara);
            Glut.glutPostRedisplay();
        }

        static void keyboardUp(byte key, int x, int y)
        {
            char k = (char)key;
            switch (k)
            {
                case 'w':
                case 'W':
                    derechoDown = false;
                    break;
                case 'S':
                case 's':
                    atrasDown = false;
                    break;
                case 'a':
                case 'A':
                    izqDown = false;
                    break;
                case 'd':
                case 'D':
                    derDown = false;
                    break;
            }
            Glut.glutPostRedisplay();
        }

        static public float GradosARadianes(double angulo)
        {

            return (float)(Math.PI * angulo / 180.0);
        }

        static public float RadianesAGrados(double angulo)
        {

            return (float)(angulo * (180.0 / Math.PI));

        }

        static void onMotion(int x, int y)
        {
            alfa = (alfa + (y - y0));
            beta = (beta + (x - x0));
            x0 = x;
            y0 = y;
            Glut.glutPostRedisplay();

        }

        static void onMouse(int button, int state, int x, int y)
        {
            if ((button == Glut.GLUT_LEFT_BUTTON) && (state == Glut.GLUT_DOWN))
            {
                x0 = x;
                y0 = y;

            }


        }
    }
}
