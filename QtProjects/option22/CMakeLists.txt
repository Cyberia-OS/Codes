cmake_minimum_required(VERSION 3.19)
project(option22 LANGUAGES CXX)

find_package(Qt6 6.5 REQUIRED COMPONENTS Core)

qt_standard_project_setup()

qt_add_executable(option22
    main.cpp
)

target_link_libraries(option22
    PRIVATE
        Qt::Core
)

install(TARGETS option22
    BUNDLE  DESTINATION .
    RUNTIME DESTINATION ${CMAKE_INSTALL_BINDIR}
    LIBRARY DESTINATION ${CMAKE_INSTALL_LIBDIR}
)
