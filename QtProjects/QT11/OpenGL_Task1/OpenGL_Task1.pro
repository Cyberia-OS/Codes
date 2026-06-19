QT += core gui opengl openglwidgets

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = OpenGL_Task1
TEMPLATE = app

SOURCES += main.cpp \
           mainwindow.cpp \
           glwidget.cpp

HEADERS  += mainwindow.h \
            glwidget.h

FORMS    += mainwindow.ui

win32: LIBS += -lopengl32
