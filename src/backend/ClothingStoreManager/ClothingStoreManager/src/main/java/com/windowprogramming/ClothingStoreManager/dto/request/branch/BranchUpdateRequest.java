package com.windowprogramming.ClothingStoreManager.dto.request.branch;

import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import lombok.AccessLevel;
import lombok.Builder;
import lombok.Getter;
import lombok.Setter;
import lombok.experimental.FieldDefaults;

@Getter
@Setter
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
public class BranchUpdateRequest {
    @NotBlank(message = "REQUIRED_BRANCH_NAME")
    String name;

}
