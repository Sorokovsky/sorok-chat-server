import { ApiBody, ApiConsumes } from "@nestjs/swagger";
import { applyDecorators } from "@nestjs/common";

export const SwaggerFile = <T>(schema: T) => {
  return applyDecorators(
    ApiConsumes("multipart/form-data"),
    ApiBody({
      type: schema as string,
    }),
  );
};
