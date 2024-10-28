package com.windowprogramming.ClothingStoreManager.dto.response;

import com.fasterxml.jackson.annotation.JsonInclude;
import com.windowprogramming.ClothingStoreManager.entity.User;
import lombok.AccessLevel;
import lombok.Builder;
import lombok.Getter;
import lombok.Setter;
import lombok.experimental.FieldDefaults;

import java.time.LocalDate;

@Getter
@Setter
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
@JsonInclude(JsonInclude.Include.NON_NULL)
public class UserResponse {
    Long id;

    String username;

    EmployeeResponse employee;

    String phoneNumber;

    String email;

    LocalDate dateOfBirth;

    String address;

    String area;

    String ward;

    String notes;

    RoleResponse role;
}
