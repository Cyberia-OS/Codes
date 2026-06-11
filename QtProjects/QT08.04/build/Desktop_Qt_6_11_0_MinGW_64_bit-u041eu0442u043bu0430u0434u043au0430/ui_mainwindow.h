/********************************************************************************
** Form generated from reading UI file 'mainwindow.ui'
**
** Created by: Qt User Interface Compiler version 6.11.0
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_MAINWINDOW_H
#define UI_MAINWINDOW_H

#include <QtCore/QVariant>
#include <QtWidgets/QApplication>
#include <QtWidgets/QLabel>
#include <QtWidgets/QMainWindow>
#include <QtWidgets/QPushButton>
#include <QtWidgets/QTextEdit>
#include <QtWidgets/QVBoxLayout>
#include <QtWidgets/QWidget>

QT_BEGIN_NAMESPACE

class Ui_MainWindow
{
public:
    QWidget *centralwidget;
    QVBoxLayout *verticalLayout;
    QLabel *labelF;
    QTextEdit *textF;
    QPushButton *btnRead;
    QLabel *labelG;
    QTextEdit *textG;
    QPushButton *btnWrite;

    void setupUi(QMainWindow *MainWindow)
    {
        if (MainWindow->objectName().isEmpty())
            MainWindow->setObjectName("MainWindow");
        MainWindow->resize(618, 559);
        centralwidget = new QWidget(MainWindow);
        centralwidget->setObjectName("centralwidget");
        verticalLayout = new QVBoxLayout(centralwidget);
        verticalLayout->setSpacing(10);
        verticalLayout->setObjectName("verticalLayout");
        verticalLayout->setContentsMargins(10, 10, 10, 10);
        labelF = new QLabel(centralwidget);
        labelF->setObjectName("labelF");

        verticalLayout->addWidget(labelF);

        textF = new QTextEdit(centralwidget);
        textF->setObjectName("textF");
        textF->setMinimumSize(QSize(0, 150));

        verticalLayout->addWidget(textF);

        btnRead = new QPushButton(centralwidget);
        btnRead->setObjectName("btnRead");

        verticalLayout->addWidget(btnRead);

        labelG = new QLabel(centralwidget);
        labelG->setObjectName("labelG");

        verticalLayout->addWidget(labelG);

        textG = new QTextEdit(centralwidget);
        textG->setObjectName("textG");
        textG->setMinimumSize(QSize(0, 150));
        textG->setReadOnly(true);

        verticalLayout->addWidget(textG);

        btnWrite = new QPushButton(centralwidget);
        btnWrite->setObjectName("btnWrite");

        verticalLayout->addWidget(btnWrite);

        MainWindow->setCentralWidget(centralwidget);

        retranslateUi(MainWindow);

        QMetaObject::connectSlotsByName(MainWindow);
    } // setupUi

    void retranslateUi(QMainWindow *MainWindow)
    {
        MainWindow->setWindowTitle(QCoreApplication::translate("MainWindow", "MainWindow", nullptr));
        labelF->setText(QCoreApplication::translate("MainWindow", "\320\244\320\260\320\271\320\273 f (\320\270\321\201\321\205\320\276\320\264\320\275\321\213\320\265 \321\207\320\270\321\201\320\273\320\260):", nullptr));
        btnRead->setText(QCoreApplication::translate("MainWindow", "\320\247\320\270\321\202\320\260\321\202\321\214 \320\270\320\267 \321\204\320\260\320\271\320\273\320\260 f", nullptr));
        labelG->setText(QCoreApplication::translate("MainWindow", "\320\244\320\260\320\271\320\273 g (\320\274\320\260\320\272\321\201\320\270\320\274\321\203\320\274\321\213 \320\277\320\276 \320\263\321\200\321\203\320\277\320\277\320\260\320\274 \320\270\320\267 5):", nullptr));
        btnWrite->setText(QCoreApplication::translate("MainWindow", "\320\236\320\261\321\200\320\260\320\261\320\276\321\202\320\260\321\202\321\214 \320\270 \320\267\320\260\320\277\320\270\321\201\320\260\321\202\321\214 \320\262 \321\204\320\260\320\271\320\273 g", nullptr));
    } // retranslateUi

};

namespace Ui {
    class MainWindow: public Ui_MainWindow {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_MAINWINDOW_H
