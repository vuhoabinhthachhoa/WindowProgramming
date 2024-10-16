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
    INVALID_USERNAME(2001, HttpStatus.BAD_REQUEST, "Username should be at least {} characters"),
    INVALID_PASSWORD(2002, HttpStatus.BAD_REQUEST, "Password should be at least {} characters, " +
            "contain at least one uppercase letter, one lowercase letter, one number, and one special character"),
    REQUIRED_EMAIL(2003, HttpStatus.BAD_REQUEST, "Email is required"),
    SAME_PASSWORD(2004, HttpStatus.BAD_REQUEST, "New password must be different from the old password"),
    //    Existed Error 3xxx
    USER_EXISTED(3001, HttpStatus.BAD_REQUEST, "User existed"),
    USER_PROFILE_EXISTED(3002, HttpStatus.BAD_REQUEST, "User profile existed"),
    //    Not Found Error 4xxx
    USER_NOT_FOUND(4001, HttpStatus.NOT_FOUND, "User not found"),
    ROLE_NOT_FOUND(4002, HttpStatus.NOT_FOUND, "Role not found"),
    USER_PROFILE_NOT_FOUND(4003, HttpStatus.NOT_FOUND, "User profile not found"),
    PERMISSION_NOT_FOUND(4004, HttpStatus.NOT_FOUND, "Permission not found"),
    //    Unauthorized	Client	5xxx (Unauthenticated error)
    INVALID_TOKEN(5001, HttpStatus.UNAUTHORIZED, "Invalid token"),
    INVALID_USERNAME_PASSWORD(5002, HttpStatus.UNAUTHORIZED, "Invalid username or password"),
    ACCOUNT_NOT_VERIFIED(5003, HttpStatus.UNAUTHORIZED, "Account not verified"),
    INCORRECT_PASSWORD(5004, HttpStatus.UNAUTHORIZED, "Incorrect password"),
    UNAUTHENTICATED(5005, HttpStatus.UNAUTHORIZED, "Authentication failed"),
    //    Forbidden	Client	6xxx (Unauthorized error)
    UNAUTHORIZED(6001, HttpStatus.FORBIDDEN, "Don't have permission"),
    ;
    final Integer code;
    final HttpStatus status;
    final String message;
}

