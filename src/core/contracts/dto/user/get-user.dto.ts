import { ApiProperty } from "@nestjs/swagger";
import { BaseGetDto } from "@contracts/dto/base-get.dto";

export class GetUserDto extends BaseGetDto {
  @ApiProperty({ default: "Sorokovskys@ukr.net" })
  public email: string;

  @ApiProperty({ default: "Andrey" })
  public name: string;

  @ApiProperty({ default: "Sorokovsky" })
  public surname: string;

  @ApiProperty({ default: "Ivanovich" })
  public middleName: string;

  @ApiProperty({ default: "1\\images\\avatar.png" })
  public avatarPath: string;
}

export class GetUserDtoWithPassword extends GetUserDto {
  @ApiProperty({ default: "password" })
  public password: string;
}
