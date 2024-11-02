package com.windowprogramming.ClothingStoreManager.exception;

import lombok.AccessLevel;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.experimental.FieldDefaults;
import org.springframework.http.HttpStatus;

@Getter
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public enum ErrorCode {
    //    Internal Server Error Undefined	9999
    UNCATEGORIZED_EXCEPTION(9999, HttpStatus.INTERNAL_SERVER_ERROR, "Uncategorized Error"),

    //    Internal Server Error	Developer error 1xxx
    INVALID_KEY(1001, HttpStatus.INTERNAL_SERVER_ERROR, "Invalid Key"),

    //    Bad Request Client Input Error 2xxx
    REQUIRED_BRANCH_NAME(2001, HttpStatus.BAD_REQUEST, "Branch name is required"),
    REQUIRED_CATEGORY_CODE(2002, HttpStatus.BAD_REQUEST, "Category code is required"),
    REQUIRED_CATEGORY_NAME(2003, HttpStatus.BAD_REQUEST, "Category name is required"),
    REQUIRED_PRODUCT_CODE(2004, HttpStatus.BAD_REQUEST, "Product code is required"),
    REQUIRED_PRODUCT_NAME(2005, HttpStatus.BAD_REQUEST, "Product name is required"),
    REQUIRED_CATEGORY_ID(2006, HttpStatus.BAD_REQUEST, "Category ID is required"),
    REQUIRED_IMPORT_PRICE(2007, HttpStatus.BAD_REQUEST, "Import price is required"),
    REQUIRED_SELLING_PRICE(2008, HttpStatus.BAD_REQUEST, "Selling price is required"),
    REQUIRED_BRANCH_ID(2009, HttpStatus.BAD_REQUEST, "Branch ID is required"),
    REQUIRED_INVENTORY_QUANTITY(2010, HttpStatus.BAD_REQUEST, "Inventory quantity is required"),
    REQUIRED_BUSINESS_STATUS(2011, HttpStatus.BAD_REQUEST, "Business status is required"),
    REQUIRED_CUSTOMER_NAME(2012, HttpStatus.BAD_REQUEST, "Customer name is required"),
    REQUIRED_CUSTOMER_ID(2013, HttpStatus.BAD_REQUEST, "Customer ID is required"),
    REQUIRED_EMPLOYEE_NAME(2014, HttpStatus.BAD_REQUEST, "Employee name is required"),
    REQUIRED_EMPLOYEE_CITIZEN_ID(2015, HttpStatus.BAD_REQUEST, "Citizen ID is required"),
    REQUIRED_EMPLOYEE_JOB_TITLE(2016, HttpStatus.BAD_REQUEST, "Job title is required"),
    REQUIRED_EMPLOYEE_SALARY(2017, HttpStatus.BAD_REQUEST, "Salary is required"),
    REQUIRED_EMPLOYEE_ID(2018, HttpStatus.BAD_REQUEST, "Employee ID is required"),
    REQUIRED_CREATED_DATE(2019, HttpStatus.BAD_REQUEST, "Created date is required"),
    REQUIRED_TOTAL_AMOUNT(2020, HttpStatus.BAD_REQUEST, "Total amount is required"),
    REQUIRED_REAL_AMOUNT(2021, HttpStatus.BAD_REQUEST, "Real amount is required"),
    REQUIRED_INVOICE_ID(2022, HttpStatus.BAD_REQUEST, "Invoice ID is required"),
    REQUIRED_PRODUCT_ID(2023, HttpStatus.BAD_REQUEST, "Product ID is required"),
    REQUIRED_PRODUCT_NAME_IN_INVOICE(2024, HttpStatus.BAD_REQUEST, "Product name in invoice is required"),
    REQUIRED_CATEGORY_ID_IN_INVOICE(2025, HttpStatus.BAD_REQUEST, "Category ID in invoice is required"),
    REQUIRED_IMPORT_PRICE_IN_INVOICE(2026, HttpStatus.BAD_REQUEST, "Import price in invoice is required"),
    REQUIRED_SELLING_PRICE_IN_INVOICE(2027, HttpStatus.BAD_REQUEST, "Selling price in invoice is required"),
    REQUIRED_BRANCH_ID_IN_INVOICE(2028, HttpStatus.BAD_REQUEST, "Branch ID in invoice is required"),
    REQUIRED_USERNAME(2029, HttpStatus.BAD_REQUEST, "Username is required"),
    REQUIRED_PASSWORD(2030, HttpStatus.BAD_REQUEST, "Password is required"),
    REQUIRED_ROLE_NAME(2031, HttpStatus.BAD_REQUEST, "Role name is required"),
    INVALID_USERNAME(2032, HttpStatus.BAD_REQUEST, "Username should be at least {} characters"),
    INVALID_PASSWORD(2033, HttpStatus.BAD_REQUEST, "Password should be at least {} characters, " +
            "contain at least one uppercase letter, one lowercase letter, one number, and one special character"),
    SAME_PASSWORD(2034, HttpStatus.BAD_REQUEST, "New password must be different from the old password"),
    REQUIRED_NEW_PASSWORD(2035, HttpStatus.BAD_REQUEST, "New password is required"),
    REQUIRED_OLD_PASSWORD(2036, HttpStatus.BAD_REQUEST, "Old password is required"),
    REQUIRED_TOKEN(2037, HttpStatus.BAD_REQUEST, "Token is required"),
    REQUIRED_USER_ID(2038, HttpStatus.BAD_REQUEST, "User ID is required"),
    REQUIRED_SIZE(2039, HttpStatus.BAD_REQUEST, "Size is required"),

    //    Existed Error 3xxx
    USER_EXISTED(3001, HttpStatus.BAD_REQUEST, "User existed"),
    USER_PROFILE_EXISTED(3002, HttpStatus.BAD_REQUEST, "User profile existed"),
    EMPLOYEE_EXISTED(3003, HttpStatus.BAD_REQUEST, "Employee existed"),
    BRANCH_EXISTED(3004, HttpStatus.BAD_REQUEST, "Branch existed"),
    CATEGORY_EXISTED(3005, HttpStatus.BAD_REQUEST, "Category existed"),
    PRODUCT_EXISTED(3006, HttpStatus.BAD_REQUEST, "Product existed"),

    //    Not Found Error 4xxx
    USER_NOT_FOUND(4001, HttpStatus.NOT_FOUND, "User not found"),
    ROLE_NOT_FOUND(4002, HttpStatus.NOT_FOUND, "Role not found"),
    USER_PROFILE_NOT_FOUND(4003, HttpStatus.NOT_FOUND, "User profile not found"),
    PERMISSION_NOT_FOUND(4004, HttpStatus.NOT_FOUND, "Permission not found"),
    EMPLOYEE_NOT_FOUND(4005,HttpStatus.NOT_FOUND, "Employee not found" ),
    BRAND_NOT_FOUND(4006, HttpStatus.NOT_FOUND, "Brand not found"),
    CATEGORY_NOT_FOUND(4007, HttpStatus.NOT_FOUND, "Category not found"),
    PRODUCT_NOT_FOUND(4008, HttpStatus.NOT_FOUND, "Product not found"),
    INVOICE_NOT_FOUND(4009, HttpStatus.NOT_FOUND, "Invoice not found"),
    CUSTOMER_NOT_FOUND(4010, HttpStatus.NOT_FOUND, "Customer not found"),
    INVOICE_DETAIL_NOT_FOUND(4011, HttpStatus.NOT_FOUND, "Invoice detail not found"),
    BRANCH_NOT_FOUND(4012, HttpStatus.NOT_FOUND, "Branch not found"),

    //    Unauthorized	Client	5xxx (Unauthenticated error)
    INVALID_TOKEN(5001, HttpStatus.UNAUTHORIZED, "Invalid token"),
    INVALID_USERNAME_PASSWORD(5002, HttpStatus.UNAUTHORIZED, "Invalid username or password"),
    ACCOUNT_NOT_VERIFIED(5003, HttpStatus.UNAUTHORIZED, "Account not verified"),
    INCORRECT_PASSWORD(5004, HttpStatus.UNAUTHORIZED, "Incorrect password"),
    UNAUTHENTICATED(5005, HttpStatus.UNAUTHORIZED, "Authentication failed"),

    //    Forbidden	Client	6xxx (Unauthorized error)
    UNAUTHORIZED(6001, HttpStatus.FORBIDDEN, "Don't have permission"),


    //    File error 7xxx
    FILE_SIZE_TOO_LARGE(7001, HttpStatus.BAD_REQUEST, "Max file size is 2MB"),
    FILE_TYPE_NOT_ALLOWED(7002, HttpStatus.BAD_REQUEST, "Only jpg, png, gif, bmp files are allowed"),
    FILE_UPLOAD_FAILED(7003, HttpStatus.INTERNAL_SERVER_ERROR, "File upload failed");


    final Integer code;
    final HttpStatus status;
    final String message;
}

