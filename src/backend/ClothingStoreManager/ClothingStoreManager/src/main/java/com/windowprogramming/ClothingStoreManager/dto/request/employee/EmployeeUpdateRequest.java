package com.windowprogramming.ClothingStoreManager.dto.request.employee;

import jakarta.validation.constraints.NotNull;
import lombok.*;
import lombok.experimental.FieldDefaults;

import java.math.BigDecimal;
import java.time.LocalDate;

@Getter
@Setter
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
public class EmployeeUpdateRequest {
    @NotNull(message = "REQUIRED_EMPLOYEE_ID")
    Long id;

    @NotNull(message = "REQUIRED_EMPLOYEE_NAME")
    String name;

    String phoneNumber;

    @NotNull(message = "REQUIRED_EMPLOYEE_CITIZEN_ID")
    String citizenId;

    @NotNull(message = "REQUIRED_EMPLOYEE_JOB_TITLE")
    String jobTitle;

    @NotNull(message = "REQUIRED_EMPLOYEE_SALARY")
    BigDecimal salary;
    String email;
    LocalDate dateOfBirth;
    String address;
    String area;
    String ward;
}