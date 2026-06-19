#include "glwidget.h"

GLWidget::GLWidget(QWidget *parent) :
    QOpenGLWidget(parent)
{
    //значения
    amplitude = 50;
    frequency = 3;
}

void GLWidget::changeAmplitude(int value)
{
    amplitude = value;
    update();
}

//слайдер частоты передал новое значение
void GLWidget::changeFrequency(int value)
{
    frequency = value;
    update();
}

void GLWidget::initializeGL()
{
    initializeOpenGLFunctions();

    glClearColor(0, 0, 0, 1);

    // Задаём систему координат: X от -200 до 200, Y от -150 до 150
    glMatrixMode(GL_PROJECTION);
    glLoadIdentity();
    glOrtho(-200, 200, -150, 150, -1, 1);

    glMatrixMode(GL_MODELVIEW);
    glLoadIdentity();
}

void GLWidget::resizeGL(int w, int h)
{
    glViewport(0, 0, (GLint)w, (GLint)h);
}

void GLWidget::paintGL()
{
    glClear(GL_COLOR_BUFFER_BIT);

    glPushMatrix();

    glColor3f(0.5f, 0.5f, 0.5f);

    glBegin(GL_LINES);
        glVertex3f(-190, 0, 0); //начало оси X
        glVertex3f( 190, 0, 0); //конец оси X
        glVertex3f(0, -140, 0); //начало оси Y
        glVertex3f(0,  140, 0); //конец оси Y
    glEnd();

    //график синуса зелёным цветом
    glColor3f(0, 1, 0);

    glBegin(GL_LINE_STRIP);
    for (int x = -190; x <= 190; x++)
    {
        //y = амплитуда * sin(x * частота * маленькое число)
        float y = amplitude * sin(x * frequency * 0.05f);
        glVertex3f(x, y, 0);
    }
    glEnd();

    glPopMatrix();
}
