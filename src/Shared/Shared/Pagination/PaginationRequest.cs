﻿namespace Shared.Pagination;

public record PaginationRequest(int pageIndex = 0, int pageSize = 10);