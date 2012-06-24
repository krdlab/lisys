#pragma once

typedef int    integer;
typedef double doublereal;

extern "C" {

    doublereal dasum_(integer *n, doublereal *dx, integer *incx);

    doublereal dnrm2_(integer *n, doublereal *x, integer *incx);

    int dscal_(integer *n, doublereal *da, doublereal *dx, integer *incx);

    int daxpy_(integer *n, doublereal *da, doublereal *dx, integer *incx, doublereal *dy, integer *incy);

    doublereal ddot_(integer *n, doublereal *dx, integer *incx, doublereal *dy, integer *incy);

    int dgemv_(char *trans, integer *m, integer *n,
        doublereal *alpha,
        doublereal *a, integer *lda,
        doublereal *x, integer *incx, 
        doublereal *beta,
        doublereal *y, integer *incy);

    int dgesv_(integer *n, integer *nrhs,
        doublereal *a, integer *lda,
        integer *ipiv,
        doublereal *b, integer *ldb,
        integer *info);

    int dgesvd_(char *jobu, char *jobvt, integer *m, integer *n,
        doublereal *a, integer *lda, doublereal *s, doublereal *u, integer *
        ldu, doublereal *vt, integer *ldvt, doublereal *work, integer *lwork, 
        integer *info);

    int dgeev_(char *jobvl, char *jobvr, integer *n, doublereal *a, integer *lda, doublereal *wr, doublereal *wi, doublereal *vl, 
        integer *ldvl, doublereal *vr, integer *ldvr, doublereal *work, 
        integer *lwork, integer *info);

    int dgetrf_(integer *m, integer *n, doublereal *a, integer *lda, integer *ipiv, integer *info);

    int dgemm_(char *transa, char *transb, integer *m, integer *n, integer *k, doublereal *alpha, doublereal *a, integer *lda, 
        doublereal *b, integer *ldb, doublereal *beta, doublereal *c__, 
        integer *ldc);
}
