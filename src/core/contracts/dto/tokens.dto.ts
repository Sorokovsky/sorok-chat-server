import { ApiProperty } from "@nestjs/swagger";

export class TokensDto {
  @ApiProperty({ default: "<ACCESS_TOKEN>" })
  public accessToken: string;
  @ApiProperty({ default: "<REFRESH_TOKEN>" })
  public refreshToken: string;
}
