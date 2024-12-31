import { ApiProperty } from "@nestjs/swagger";

export class BaseGetDto {
  @ApiProperty({ default: 1 })
  public id: number;

  @ApiProperty({ default: Date.now() })
  public createdAt: Date;

  @ApiProperty({ default: Date.now() })
  public updatedAt: Date;
}
