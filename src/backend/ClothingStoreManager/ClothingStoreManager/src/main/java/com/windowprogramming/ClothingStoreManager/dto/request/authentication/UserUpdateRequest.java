package com.windowprogramming.ClothingStoreManager.dto.request.authentication;

import jakarta.validation.constraints.NotNull;
import lombok.AccessLevel;
import lombok.Builder;
import lombok.Getter;
import lombok.Setter;
import lombok.experimental.FieldDefaults;
import lombok.experimental.NonFinal;

import java.time.LocalDate;

@Getter
@Setter
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
public class UserUpdateRequest {
    @NotNull(message = "REQUIRED_USER_ID")
    Long id;
    String phoneNumber;

    String email;

    LocalDate dateOfBirth;

    String address;

    String area;

    String ward;

    String notes;
}
