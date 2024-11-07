package com.windowprogramming.ClothingStoreManager.dto.request.authentication;

import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import lombok.*;
import lombok.experimental.FieldDefaults;

@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class LoginRequest {
    @NotBlank(message = "REQUIRED_USERNAME")
    String username;

    @NotBlank(message = "REQUIRED_PASSWORD")
    String password;
}

