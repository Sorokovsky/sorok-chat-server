import { Module } from "@nestjs/common";
import { UsersService } from "@features/users/users.service";
import { UsersController } from "@features/users/users.controller";
import { TypeOrmModule } from "@nestjs/typeorm";
import { UserEntity } from "@entities/user.entity";
import { FilesService } from "@features/files/files.service";
import { PasswordModule } from "@features/password/password.module";

@Module({
  imports: [TypeOrmModule.forFeature([UserEntity]), PasswordModule],
  controllers: [UsersController],
  providers: [UsersService, FilesService],
  exports: [UsersService],
})
export class UsersModule {}
