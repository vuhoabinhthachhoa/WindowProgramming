package com.windowprogramming.ClothingStoreManager.dto.response;

import com.fasterxml.jackson.annotation.JsonInclude;
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
@JsonInclude(JsonInclude.Include.NON_NULL)
public class EmployeeResponse {
    Long id;
    String name;
    String phoneNumber;
    String citizenId;
    String jobTitle;
    BigDecimal salary;
    String email;
    LocalDate dateOfBirth;
    String address;
    String area;
    String ward;
}
