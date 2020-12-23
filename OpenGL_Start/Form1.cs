using System;
using System.Drawing;
using System.Windows.Forms;

// для работы с библиотекой OpenGL 
using Tao.OpenGl;
// для работы с библиотекой FreeGLUT 
using Tao.FreeGlut;

namespace kg_1
{
    public partial class Form1 : Form
    {
        private float angleX = 0;
        public Form1()
        {
            InitializeComponent();
            InitializeDevice();

        }

        private void InitializeDevice()
        {
            simpleOpenGlControl1.InitializeContexts();

            // инициализация Glut 
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);

            // очитка окна 
            Gl.glClearColor(1, 1, 1, 1.0f);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            // установка порта вывода в соотвествии с размерами элемента simpleOpenGlControl1 
            Gl.glViewport(0, 0, simpleOpenGlControl1.Width, simpleOpenGlControl1.Height);
        }



        private void VertexDeclaration()
        {
            Gl.glBegin(Gl.GL_POLYGON);
            Color[] color = { Color.Yellow, Color.Red, Color.Blue, Color.Green};
            Gl.glColor3d((double)color[0].R / 256, (double)color[0].G / 256, (double)color[0].B / 256); Gl.glVertex3d(4, 4, 0); Gl.glNormal3d(0, 0, 1);
            Gl.glColor3d((double)color[1].R / 256, (double)color[1].G / 256, (double)color[1].B / 256); Gl.glVertex3d(-4, 4, 0); Gl.glNormal3d(0, 0, 1);
            Gl.glColor3d((double)color[2].R / 256, (double)color[2].G / 256, (double)color[2].B / 256); Gl.glVertex3d(-4, -4, 0); Gl.glNormal3d(0, 0, 1);
            Gl.glColor3d((double)color[3].R / 256, (double)color[3].G / 256, (double)color[3].B / 256); Gl.glVertex3d(4, -4, 0); Gl.glNormal3d(0, 0, 1);
            Gl.glEnd();
        }

        private void SetCamera()
        {
            // настройка проекции 
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(80, (float)simpleOpenGlControl1.Width / (float)simpleOpenGlControl1.Height, 1, 80);

            //видовое преобразование - установка камеры
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Glu.gluLookAt(20, 20, 20, 0, 0, 0, 0, 1, 0);
            // настройка параметров OpenGL для визуализации 
            Gl.glEnable(Gl.GL_DEPTH_TEST);

            //сглаживание
            Gl.glShadeModel(Gl.GL_SMOOTH);

            //установка света
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, new float[] {1,1,1, 0f });

            //установка материала 
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);



        }

        private void MoveCamera()
        {
            Glu.gluLookAt(20, 20, 20, 0, 0, 0, 0, 1, 0);
            Gl.glRotatef(angleX, 1, 0, 0);
            angleX += 2;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetCamera();

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //функция выполняется один раз - как только отрисовывается окно
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            Gl.glLoadIdentity();
            Gl.glColor3f(1.0f, 0, 0);

            //преобразование - трансляция, поворот и масштабирование
            Gl.glPushMatrix();

            VertexDeclaration();

            Gl.glPopMatrix();


            // завершаем режим рисования примитивов 
            Gl.glEnd();

            // дожидаемся завершения визуализации кадра 
            Gl.glFlush();

            //перерисовка
            simpleOpenGlControl1.Invalidate();

            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //таймер вызывает событие Tick раз в timer1.Interval миллисекунд - используем для перерисовки
            timer1.Stop();

            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            Gl.glLoadIdentity();
            Gl.glColor3f(1.0f, 0, 0);

            Gl.glFlush();

            //движение камеры и переобъявление вершин
            MoveCamera();
            VertexDeclaration();


            Gl.glFlush();
            simpleOpenGlControl1.Invalidate();
            timer1.Start();
        }
    }
}
