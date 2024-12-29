import { HttpStatus } from "@nestjs/common";
import { ApiProperty } from "@nestjs/swagger";

export class ErrorDto {
  @ApiProperty({
    example: "Error message",
  })
  public message: string;

  @ApiProperty({
    example: HttpStatus.BAD_REQUEST,
  })
  public code: HttpStatus;

  @ApiProperty({
    example: "Error",
  })
  public error: string;
}
