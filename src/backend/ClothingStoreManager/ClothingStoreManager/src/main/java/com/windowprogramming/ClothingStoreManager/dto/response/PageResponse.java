package com.windowprogramming.ClothingStoreManager.dto.response;

import lombok.AccessLevel;
import lombok.Builder;
import lombok.Getter;
import lombok.Setter;
import lombok.experimental.FieldDefaults;

import java.util.Collections;
import java.util.List;

@Getter
@Setter
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
public class PageResponse <T> {
    Integer page;
    Integer size;
    Long totalElements;
    Integer totalPages;

    @Builder.Default
    List<T> data = Collections.emptyList();
    //this means that if no data is set for the data field,
    // it will default to an empty, unmodifiable list.
    // This is a good defensive programming practice as
    // it ensures the data field is never null,
    //  reducing the likelihood of NullPointerExceptions.
}

