import { IsString } from "class-validator";
import { ApiProperty } from "@nestjs/swagger";

export class CreateMessageDto {
  @IsString()
  @ApiProperty({ default: "Hello world!" })
  public text: string;
}
