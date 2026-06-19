#ifndef GLWIDGET_H
#define GLWIDGET_H

#include <QOpenGLWidget>
#include <QOpenGLFunctions>
#include <cmath>

class GLWidget : public QOpenGLWidget, protected QOpenGLFunctions
{
    Q_OBJECT

public:
    explicit GLWidget(QWidget *parent = nullptr);

    void initializeGL() override;
    void paintGL() override;
    void resizeGL(int w, int h) override;

private:
    int amplitude;
    int frequency;

public slots:
    void changeAmplitude(int value);
    void changeFrequency(int value);
};

#endif // GLWIDGET_H
