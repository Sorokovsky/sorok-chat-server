import {
  Body,
  Controller,
  Post,
  UploadedFile,
  UseInterceptors,
} from "@nestjs/common";
import { AuthService } from "./auth.service";
import { GetUserDto } from "../../core/contracts/dto/user/get-user.dto";
import { SwaggerFile } from "../../core/decorators/swagger-file.decorator";
import { FileInterceptor } from "@nestjs/platform-express";
import { ApiBadRequestResponse, ApiCreatedResponse } from "@nestjs/swagger";
import { ErrorDto } from "../../core/contracts/dto/error.dto";
import {
  CreateUserDto,
  CreateUserDtoWithoutAvatar,
} from "../../core/contracts/dto/user/create-user.dto";

@Controller("auth")
export class AuthController {
  constructor(private readonly authService: AuthService) {}

  @Post("register")
  @SwaggerFile(CreateUserDto)
  @UseInterceptors(FileInterceptor("avatar"))
  @ApiCreatedResponse({
    type: GetUserDto,
    description: "Created user successfully",
  })
  @ApiBadRequestResponse({
    description: "User already exists",
    type: ErrorDto,
  })
  public async register(
    @Body() user: CreateUserDtoWithoutAvatar,
    @UploadedFile() avatar?: Express.Multer.File,
  ): Promise<GetUserDto> {
    return await this.authService.registrate(user, avatar);
  }
}
