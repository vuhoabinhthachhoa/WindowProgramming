package com.windowprogramming.ClothingStoreManager.dto.request.authentication;

import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Pattern;
import jakarta.validation.constraints.Size;
import lombok.*;
import lombok.experimental.FieldDefaults;

@Getter
@Setter
@Builder
@AllArgsConstructor
@NoArgsConstructor
@FieldDefaults(level = lombok.AccessLevel.PRIVATE)
public class ChangePasswordRequest {
    @NotNull(message = "REQUIRED_OLD_PASSWORD")
    String oldPassword;

    @NotNull(message = "REQUIRED_NEW_PASSWORD")
    @Size(min = 8, message = "INVALID_PASSWORD")
    @Pattern(regexp = "^(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&*(){}\\[\\]!~`|])(?=.*\\d).*$", message = "INVALID_PASSWORD")
    String newPassword;
}
