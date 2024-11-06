package com.windowprogramming.ClothingStoreManager.dto.request.authentication;

import jakarta.validation.constraints.NotNull;
import lombok.*;
import lombok.experimental.FieldDefaults;

@Getter
@Setter
@Builder
@AllArgsConstructor
@NoArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class LogoutRequest {
    @NotNull(message = "REQUIRED_TOKEN")
    String token;
}
