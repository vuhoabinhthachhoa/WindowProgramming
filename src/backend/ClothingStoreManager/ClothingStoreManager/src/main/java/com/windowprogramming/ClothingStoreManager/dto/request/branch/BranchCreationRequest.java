package com.windowprogramming.ClothingStoreManager.dto.request.branch;

import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import lombok.*;
import lombok.experimental.FieldDefaults;

@Getter
@Setter
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
public class BranchCreationRequest {
    @NotBlank(message = "REQUIRED_BRANCH_NAME")
    String name;

    //  if there is only one field (name), there will be a json parse error
    // this is a bug that I still don't know how to fix, so I add another field to fix it
    // this field has no uses
    String code;

}
