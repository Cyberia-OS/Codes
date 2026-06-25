QT += core gui sql widgets
CONFIG += c++17

TARGET = PayrollApp
TEMPLATE = app

SOURCES += \
    main.cpp \
    mainwindow.cpp \
    employeedialog.cpp \
    logindialog.cpp

HEADERS += \
    mainwindow.h \
    employeedialog.h \
    models.h \
    logindialog.h

FORMS += \
    mainwindow.ui \
    employeedialog.ui \
    logindialog.ui

RESOURCES += resources.qrc