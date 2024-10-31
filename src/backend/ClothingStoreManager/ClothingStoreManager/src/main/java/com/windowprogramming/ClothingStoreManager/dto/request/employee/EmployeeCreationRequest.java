package com.windowprogramming.ClothingStoreManager.dto.request.employee;

import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import lombok.AccessLevel;
import lombok.Builder;
import lombok.Getter;
import lombok.Setter;
import lombok.experimental.FieldDefaults;

import java.math.BigDecimal;
import java.time.LocalDate;

@Getter
@Setter
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
public class EmployeeCreationRequest {
    @NotBlank(message = "REQUIRED_EMPLOYEE_NAME")
    String name;

    String phoneNumber;

    @NotBlank(message = "REQUIRED_EMPLOYEE_CITIZEN_ID")
    String citizenId;

    @NotBlank(message = "REQUIRED_EMPLOYEE_JOB_TITLE")
    String jobTitle;

    @NotNull(message = "REQUIRED_EMPLOYEE_SALARY")
    BigDecimal salary;
    String email;
    LocalDate dateOfBirth;
    String address;
    String area;
    String ward;
}
