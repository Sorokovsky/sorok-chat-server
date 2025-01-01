import { BaseGetDto } from "@contracts/dto/base-get.dto";
import { ApiProperty } from "@nestjs/swagger";
import { GetUserDto } from "@contracts/dto/user/get-user.dto";

export class GetMessageDto extends BaseGetDto {
  @ApiProperty({ default: "Hello world!" })
  public text: string;

  @ApiProperty({ type: GetUserDto })
  public author: GetUserDto;
}
