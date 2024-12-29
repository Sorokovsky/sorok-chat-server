import { Body, Controller, Post, UseInterceptors } from "@nestjs/common";
import { UsersService } from "./users.service";
import { SwaggerFile } from "../../core/decorators/swagger-file.decorator";
import {
  CreateUserDto,
  CreateUserDtoWithoutAvatar,
} from "../../core/contracts/dto/user/create-user.dto";
import { FileInterceptor } from "@nestjs/platform-express";

@Controller("users")
export class UsersController {
  constructor(private readonly usersService: UsersService) {}

  @Post()
  @SwaggerFile(CreateUserDto)
  @UseInterceptors(FileInterceptor("avatar"))
  public async create(
    @Body() user: CreateUserDtoWithoutAvatar,
    file?: Express.Multer.File,
  ) {
    return await this.usersService.create(user, file);
  }
}
