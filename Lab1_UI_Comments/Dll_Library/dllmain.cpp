// dllmain.cpp : Определяет точку входа для приложения DLL.
#include "pch.h"
#include <mkl_df.h>

extern "C" _declspec(dllexport)
int do_splines(double a, double b, int length, double* nodes, double* values, double left_der, double right_der, int new_length, double* res_arr)
{
    DFTaskPtr task = NULL;
    int status;
    if ((status = dfdNewTask1D(&task, length, nodes, DF_NON_UNIFORM_PARTITION, 1, values, DF_MATRIX_STORAGE_ROWS) != DF_STATUS_OK))
    {
        return status;
    }
    double* scoeff = new double[2 * DF_PP_CUBIC * (length - 1)];
    double* derivatives = new double[2];
    derivatives[0] = left_der;
    derivatives[1] = right_der;
    if ((status = dfdEditPPSpline1D(task, DF_PP_CUBIC, DF_PP_NATURAL, DF_BC_1ST_LEFT_DER | DF_BC_1ST_RIGHT_DER, derivatives, DF_NO_IC, NULL, scoeff, DF_MATRIX_STORAGE_ROWS) != DF_STATUS_OK))
    {
        return status;
    }
    if ((status = dfdConstruct1D(task, DF_PP_SPLINE, DF_METHOD_STD)) != DF_STATUS_OK)
    {
        return status;
    }
    MKL_INT spline_ders[3] = {1, 1, 1};
    double new_nodes[2] = { a, b };
    if ((status = dfdInterpolate1D(task, DF_INTERP, DF_METHOD_PP, new_length, new_nodes, DF_UNIFORM_PARTITION, 3, spline_ders, NULL, res_arr, DF_MATRIX_STORAGE_ROWS, NULL)) != DF_STATUS_OK)
    {
        return status;
    }
    double left = a;
    double right = b;
    if ((status = dfdIntegrate1D(task, DF_METHOD_PP, 1, &left, DF_NO_HINT, &right, DF_NO_HINT, NULL, NULL, res_arr + 3 * new_length, DF_MATRIX_STORAGE_ROWS)) != DF_STATUS_OK)
    {
        return status;
    }
    status = DF_STATUS_OK;
    return status;
}

extern "C" _declspec(dllexport)
int f(int a, int b)
{
    return 2 * a + 3 * b;
}

