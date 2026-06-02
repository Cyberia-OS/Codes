/********************************************************************************
** Form generated from reading UI file 'mainwindow.ui'
**
** Created by: Qt User Interface Compiler version 6.10.2
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_MAINWINDOW_H
#define UI_MAINWINDOW_H

#include <QtCore/QVariant>
#include <QtWidgets/QApplication>
#include <QtWidgets/QLabel>
#include <QtWidgets/QLineEdit>
#include <QtWidgets/QMainWindow>
#include <QtWidgets/QMenuBar>
#include <QtWidgets/QPushButton>
#include <QtWidgets/QStatusBar>
#include <QtWidgets/QWidget>

QT_BEGIN_NAMESPACE

class Ui_MainWindow
{
public:
    QWidget *centralwidget;
    QLabel *label_A;
    QLineEdit *lineEdit_A;
    QLabel *label_B;
    QLineEdit *lineEdit_B;
    QLabel *label_C;
    QLineEdit *lineEdit_C;
    QPushButton *pushButton;
    QLabel *label_result;
    QMenuBar *menubar;
    QStatusBar *statusbar;

    void setupUi(QMainWindow *MainWindow)
    {
        if (MainWindow->objectName().isEmpty())
            MainWindow->setObjectName("MainWindow");
        MainWindow->resize(300, 250);
        centralwidget = new QWidget(MainWindow);
        centralwidget->setObjectName("centralwidget");
        label_A = new QLabel(centralwidget);
        label_A->setObjectName("label_A");
        label_A->setGeometry(QRect(20, 20, 60, 25));
        lineEdit_A = new QLineEdit(centralwidget);
        lineEdit_A->setObjectName("lineEdit_A");
        lineEdit_A->setGeometry(QRect(90, 20, 180, 25));
        label_B = new QLabel(centralwidget);
        label_B->setObjectName("label_B");
        label_B->setGeometry(QRect(20, 60, 60, 25));
        lineEdit_B = new QLineEdit(centralwidget);
        lineEdit_B->setObjectName("lineEdit_B");
        lineEdit_B->setGeometry(QRect(90, 60, 180, 25));
        label_C = new QLabel(centralwidget);
        label_C->setObjectName("label_C");
        label_C->setGeometry(QRect(20, 100, 60, 25));
        lineEdit_C = new QLineEdit(centralwidget);
        lineEdit_C->setObjectName("lineEdit_C");
        lineEdit_C->setGeometry(QRect(90, 100, 180, 25));
        pushButton = new QPushButton(centralwidget);
        pushButton->setObjectName("pushButton");
        pushButton->setGeometry(QRect(90, 140, 180, 30));
        label_result = new QLabel(centralwidget);
        label_result->setObjectName("label_result");
        label_result->setGeometry(QRect(20, 185, 260, 45));
        label_result->setAlignment(Qt::AlignCenter);
        MainWindow->setCentralWidget(centralwidget);
        menubar = new QMenuBar(MainWindow);
        menubar->setObjectName("menubar");
        menubar->setGeometry(QRect(0, 0, 300, 21));
        MainWindow->setMenuBar(menubar);
        statusbar = new QStatusBar(MainWindow);
        statusbar->setObjectName("statusbar");
        MainWindow->setStatusBar(statusbar);

        retranslateUi(MainWindow);

        QMetaObject::connectSlotsByName(MainWindow);
    } // setupUi

    void retranslateUi(QMainWindow *MainWindow)
    {
        MainWindow->setWindowTitle(QCoreApplication::translate("MainWindow", "\320\227\320\260\320\264\320\260\321\207\320\260 1", nullptr));
        label_A->setText(QCoreApplication::translate("MainWindow", "\320\247\320\270\321\201\320\273\320\276 A:", nullptr));
        label_B->setText(QCoreApplication::translate("MainWindow", "\320\247\320\270\321\201\320\273\320\276 B:", nullptr));
        label_C->setText(QCoreApplication::translate("MainWindow", "\320\247\320\270\321\201\320\273\320\276 C:", nullptr));
        pushButton->setText(QCoreApplication::translate("MainWindow", "\320\222\321\213\321\207\320\270\321\201\320\273\320\270\321\202\321\214", nullptr));
        label_result->setText(QCoreApplication::translate("MainWindow", "\320\240\320\265\320\267\321\203\320\273\321\214\321\202\320\260\321\202 \320\277\320\276\321\217\320\262\320\270\321\202\321\201\321\217 \320\267\320\264\320\265\321\201\321\214", nullptr));
    } // retranslateUi

};

namespace Ui {
    class MainWindow: public Ui_MainWindow {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_MAINWINDOW_H
