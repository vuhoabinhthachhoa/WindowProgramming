package com.windowprogramming.ClothingStoreManager.dto.response;

import com.fasterxml.jackson.annotation.JsonInclude;
import com.windowprogramming.ClothingStoreManager.entity.User;
import lombok.AccessLevel;
import lombok.Builder;
import lombok.Getter;
import lombok.Setter;
import lombok.experimental.FieldDefaults;

@Getter
@Setter
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
@JsonInclude(JsonInclude.Include.NON_NULL)
public class LoginResponse {
    String token;
}
